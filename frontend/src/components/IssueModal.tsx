import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import toast from 'react-hot-toast';

export type Member = {
  userId: string;
  userName: string;
  role: string;
  pictureUrl: string;
};

export interface IssueModalProps {
  onClose: () => void;
  onSave: (data: {
    id?: string;
    title: string;
    description: string;
    assignerUserId: string;
    assignedUserId: string;
    completeDate: string;
    projectId: string;
  }) => void;
  projectId: string;
  members: Member[];
  initialData?: {
    id?: string;
    title?: string;
    description?: string;
    assignerUserId?: string;
    assignedUserId?: string;
    completeDate?: string;
    mode?: 'edit' | 'view';
  };
}

export function IssueModal({ onClose, onSave, projectId, members = [], initialData }: IssueModalProps) {
  const [title, setTitle] = useState(initialData?.title ?? '');
  const [description, setDescription] = useState(initialData?.description ?? '');
  
  
  const [completeDate, setCompleteDate] = useState(initialData?.completeDate ?? '');
  const { user } = useAuth();
  const isViewOnly = initialData?.mode === 'view';
  const [assignedUserError, setAssignedUserError] = useState('');
  const [assignedUserId, setAssignedUserId] = useState(
  initialData?.assignedUserId ?? user?.id ?? ''
);

console.log('IssueModal props:', { projectId, members, initialData });
console.log('Members in IssueModal:', members);

  const handleSubmit = () => {
  if (!title.trim()) return;
  if (!assignedUserId) {
    setAssignedUserError('Выберите исполнителя');
    return;
  }

  setAssignedUserError('');

  const data = {
    id: initialData?.id,
    title,
    description,
    assignerUserId: user?.id!,
    assignedUserId,
    completeDate,
    projectId,
  };

  console.log('Submitting issue:', data);
  onSave(data);
  toast.success("Задача успешно добавлена");
};

  const getMinDateTime = () => {
  const now = new Date();
  now.setSeconds(0, 0); // Убираем секунды и миллисекунды для совместимости
  return now.toISOString().slice(0, 16); // Формат: "YYYY-MM-DDTHH:MM"
};

  return (
    <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex justify-center items-center z-50">
      <div className="bg-white p-6 rounded-2xl w-full max-w-md shadow-xl relative">
        <h2 className="text-2xl font-semibold text-gray-800 mb-4 text-center">
          {initialData?.mode === 'view'
            ? 'Просмотр задачи'
            : initialData?.id
            ? 'Редактировать задачу'
            : 'Создать задачу'}
        </h2>

        <div className="space-y-4">
          <input
            className="w-full px-4 py-2 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
            placeholder="Заголовок"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            disabled={isViewOnly}
          />

          <textarea
            className="w-full px-4 py-2 h-24 border border-gray-300 rounded-xl resize-none focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
            placeholder="Описание"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            disabled={isViewOnly}
          />

          <div className="mt-4">
  <label className="text-sm font-medium text-gray-700 block mb-1">Исполнитель</label>
  <select
    className={`w-full px-4 py-2 border rounded-xl focus:outline-none focus:ring-2 ${
      assignedUserError ? 'border-red-500 focus:ring-red-400' : 'border-gray-300 focus:ring-blue-500'
    } disabled:bg-gray-100`}
    value={assignedUserId}
    onChange={(e) => {
      setAssignedUserId(e.target.value);
      if (e.target.value) setAssignedUserError('');
    }}
    disabled={isViewOnly}
  >
    <option value="">Выберите</option>
    {members && members.map((m) => (
      <option key={m.userId} value={m.userId}>
        {m.userName}
      </option>
    ))}
  </select>
  {assignedUserError && <p className="text-red-500 text-sm mt-1">{assignedUserError}</p>}
</div>

          <div>
  <label className="text-sm font-medium text-gray-700 block mb-1">Срок выполнения</label>
  <input
    type="datetime-local"
    className="w-full px-4 py-2 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
    value={completeDate}
    onChange={(e) => setCompleteDate(e.target.value)}
    disabled={isViewOnly}
    min={getMinDateTime()}
  />
</div>
        </div>

        <div className="flex justify-end space-x-2 mt-6">
          {!isViewOnly && (
            <button
              onClick={handleSubmit}
              className="bg-blue-600 hover:bg-blue-700 text-white font-medium px-4 py-2 rounded-xl transition"
            >
              Сохранить
            </button>
          )}
          <button
            onClick={onClose}
            className="bg-gray-200 hover:bg-gray-300 text-gray-800 font-medium px-4 py-2 rounded-xl transition"
          >
            {isViewOnly ? 'Закрыть' : 'Отмена'}
          </button>
        </div>
      </div>
    </div>
  );
}