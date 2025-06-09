import React, { useState } from 'react';
import KanbanBoard from '../components/kanban/KanbanBoard.jsx';
import { ProjectChat } from '../components/ProjectChat.jsx';
import { useParams } from 'react-router-dom';

export const ProjectBoardPage = () => {
  const { projectId } = useParams();
  const [chatOpen, setChatOpen] = useState(false);

  return (
    <div className="p-4 relative">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Задачи проекта</h1>
        <button
          onClick={() => setChatOpen(true)}
          className="px-4 py-2 bg-indigo-600 text-white rounded hover:bg-indigo-700 transition"
        >
          Открыть чат
        </button>
      </div>

      <KanbanBoard projectId={projectId!} />

      {chatOpen && (
        <div className="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
          <div className="bg-white w-full max-w-3xl rounded-xl shadow-lg relative p-4">
            <button
              onClick={() => setChatOpen(false)}
              className="absolute top-2 right-2 text-gray-500 hover:text-gray-800"
            >
              ✖
            </button>
            <ProjectChat projectId={projectId} />
          </div>
        </div>
      )}
    </div>
  );
};