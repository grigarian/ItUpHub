import React, { useEffect, useState, useCallback } from "react";
import { DragDropContext, Droppable, Draggable } from "@hello-pangea/dnd";
import api from "../../api/axios";
import toast from "react-hot-toast";
import { useAuth } from '../../context/AuthContext';
import { IssueModal } from '../IssueModal';
import { Loader2, Plus, Save, Calendar, User, AlertCircle, CheckCircle } from 'lucide-react';

// Порядок отображения колонок
const statusOrder = [
  "Backlog",
  "ToDo",
  "InProgress",
  "Review",
  "Done",
];

// Переводы статусов
const statusTranslations = {
  "Backlog": "Бэклог",
  "ToDo": "К выполнению",
  "InProgress": "В работе",
  "Review": "На проверке",
  "Done": "Завершено",
};

const statusColors = {
  "Backlog": "bg-gray-200",
  "ToDo": "bg-blue-200",
  "InProgress": "bg-yellow-200",
  "Review": "bg-purple-200",
  "Done": "bg-green-200",
};

const KanbanBoard = ({ projectId }) => {
  const [columns, setColumns] = useState({});
  const [members, setMembers] = useState([]);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingIssue, setEditingIssue] = useState(null);
  const [isSaving, setIsSaving] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const { user } = useAuth();

  const fetchData = useCallback(async () => {
    try {
      setIsLoading(true);
      const [issuesRes, membersRes] = await Promise.all([
        api.get(`issue/project/${projectId}`),
        api.get(`project/${projectId}/members`, { withCredentials: true })
      ]);
      
      console.log('Issues response:', issuesRes.data);
      console.log('Members response:', membersRes.data);
      
      const formatted = {};
      statusOrder.forEach((status) => {
        formatted[status] = {
          name: status,
          items: issuesRes.data[status] || [],
        };
      });

      setColumns(formatted);
      setMembers(membersRes.data || []);
    } catch (error) {
      console.error('Error fetching data:', error);
      console.error('Error response:', error.response?.data);
      toast.error("Ошибка загрузки данных");
    } finally {
      setIsLoading(false);
    }
  }, [projectId]);

  useEffect(() => {
    fetchData();
  }, [projectId, fetchData]);

  const onDragEnd = (result) => {
    const { source, destination } = result;
    if (!destination) return;

    if (source.droppableId === destination.droppableId) {
      const column = columns[source.droppableId];
      const items = Array.from(column.items);
      const [movedItem] = items.splice(source.index, 1);
      items.splice(destination.index, 0, movedItem);

      setColumns({
        ...columns,
        [source.droppableId]: {
          ...column,
          items,
        },
      });
    } else {
      const sourceCol = columns[source.droppableId];
      const destCol = columns[destination.droppableId];

      const sourceItems = Array.from(sourceCol.items);
      const destItems = Array.from(destCol.items);

      const [movedItem] = sourceItems.splice(source.index, 1);
      movedItem.status = destination.droppableId;
      destItems.splice(destination.index, 0, movedItem);

      setColumns({
        ...columns,
        [source.droppableId]: {
          ...sourceCol,
          items: sourceItems,
        },
        [destination.droppableId]: {
          ...destCol,
          items: destItems,
        },
      });
    }
  };

  const handleSave = async () => {
    try {
      setIsSaving(true);
      const payload = statusOrder.map((status) => ({
        status,
        issueIds: columns[status].items.map((i) => i.id),
      }));

      await api.post(`issue/project/${projectId}/reorder`, {
        columns: payload,
      });
      toast.success("Изменения сохранены");
    } catch (error) {
      toast.error("Ошибка при сохранении");
    } finally {
      setIsSaving(false);
    }
  };

  const handleOpenModal = () => {
    console.log('Opening modal with members:', members);
    setEditingIssue(null);
    setModalOpen(true);
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    setEditingIssue(null);
  };

  const handleEditClick = (issue) => {
    const mode = issue.assigner === user.id ? 'edit' : 'view';
    setEditingIssue({ ...issue, mode });
    setModalOpen(true);
  };

  const handleSaveIssue = async (formData) => {
    try {
      const request = formData.id
        ? api.put(`issue/${formData.id}`, formData)
        : api.post(`issue`, formData);

      await request;
      await fetchData();
      toast.success("Задача сохранена");
      setModalOpen(false);
    } catch (error) {
      toast.error("Ошибка при сохранении задачи");
    }
  };

  const getStatusIcon = (status) => {
    switch(status) {
      case "Done": return <CheckCircle size={16} className="text-green-600" />;
      case "Review": return <AlertCircle size={16} className="text-purple-600" />;
      default: return null;
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Loader2 className="animate-spin h-12 w-12 text-indigo-600" />
      </div>
    );
  }

  return (
    <div className="bg-white/90 backdrop-blur-lg rounded-xl shadow-lg p-6 border border-gray-100">
      <div className="flex flex-wrap justify-between items-center mb-6 gap-4">
        <h2 className="text-2xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
          Доска задач
        </h2>
        <div className="flex gap-3">
          <button
            onClick={handleSave}
            disabled={isSaving}
            className="flex items-center gap-2 px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white rounded-lg shadow hover:shadow-indigo-200 transition-all disabled:opacity-70"
          >
            {isSaving ? (
              <Loader2 className="animate-spin h-4 w-4" />
            ) : (
              <Save size={16} />
            )}
            <span>Сохранить</span>
          </button>
          <button
            onClick={handleOpenModal}
            className="flex items-center gap-2 px-4 py-2 bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700 text-white rounded-lg shadow hover:shadow-green-200 transition-all"
          >
            <Plus size={16} />
            <span>Новая задача</span>
          </button>
        </div>
      </div>

      <DragDropContext onDragEnd={onDragEnd}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
          {statusOrder.map((status) => (
  <div 
    key={status} 
    className="relative bg-gray-50 rounded-xl shadow-sm border border-gray-100"
  >
    <div className={`p-3 rounded-t-xl flex justify-between items-center ${statusColors[status]}`}>
      <h3 className="font-semibold text-gray-800">
        {statusTranslations[status]}
      </h3>
      <span className="bg-white/80 text-gray-700 px-2 py-1 rounded-full text-xs">
        {columns[status]?.items?.length || 0}
      </span>
    </div>
    
    <Droppable droppableId={status}>
      {(provided) => (
        <div
          {...provided.droppableProps}
          ref={provided.innerRef}
          className="p-3 min-h-[400px]" // убедись, что нет overflow-hidden
        >
          {columns[status]?.items?.map((issue, index) => (
            <Draggable key={issue.id} draggableId={issue.id} index={index}>
              {(provided) => (
                <div
                  ref={provided.innerRef}
                  {...provided.draggableProps}
                  {...provided.dragHandleProps}
                  onClick={() => handleEditClick(issue)}
                  className="bg-white mb-3 rounded-lg border border-gray-200 shadow-sm hover:shadow-md transition-all cursor-pointer p-4"
                  style={provided.draggableProps.style} // 💡 обязательно
                >
                  <div className="flex justify-between items-start mb-2">
                    <h4 className="font-medium text-gray-900">{issue.title}</h4>
                    {getStatusIcon(issue.status)}
                  </div>
                  
                  <p className="text-gray-600 text-sm mb-3 line-clamp-2">
                    {issue.description}
                  </p>
                  
                  <div className="flex items-center justify-between text-xs">
                    <div className="flex items-center text-gray-500">
                      <Calendar size={14} className="mr-1" />
                      <span>
                        {issue.completeDate 
                          ? new Date(issue.completeDate).toLocaleDateString() 
                          : 'Нет срока'}
                      </span>
                    </div>
                    
                    {issue.assigner && (
                      <div className="flex items-center">
                        <User size={14} className="mr-1 text-gray-500" />
                        <span className="text-gray-700">
                          {members.find(m => m.userId === issue.assigner)?.userName || ''}
                        </span>
                      </div>
                    )}
                  </div>
                </div>
              )}
            </Draggable>
          ))}
          {provided.placeholder}
        </div>
      )}
    </Droppable>
  </div>
))}
        </div>
      </DragDropContext>

      {modalOpen && (
        <IssueModal
          onClose={handleCloseModal}
          onSave={handleSaveIssue}
          projectId={projectId}
          members={members}
          initialData={
            editingIssue && {
              id: editingIssue.id,
              title: editingIssue.title,
              description: editingIssue.description,
              assignerUserId: editingIssue.assigner,
              assignedUserId: editingIssue.assignedTo,
              completeDate: editingIssue.completeDate?.slice(0, 16),
              mode: editingIssue.mode,
            }
          }
        />
      )}
    </div>
  );
};

export default KanbanBoard;