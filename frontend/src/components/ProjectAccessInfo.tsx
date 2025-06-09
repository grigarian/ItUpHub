import React from 'react';
import { Link } from 'react-router-dom';
import { AlertCircle, Users, Lock } from 'lucide-react';

interface ProjectAccessInfoProps {
  projectId: string;
  projectTitle: string;
}

export const ProjectAccessInfo: React.FC<ProjectAccessInfoProps> = ({ projectId, projectTitle }) => {
  return (
    <div className="max-w-2xl mx-auto mt-8 p-6 bg-white/90 backdrop-blur-lg rounded-3xl shadow-lg border border-gray-100">
      <div className="text-center">
        <div className="flex justify-center mb-4">
          <div className="p-3 bg-orange-100 rounded-full">
            <Lock className="h-8 w-8 text-orange-600" />
          </div>
        </div>
        
        <h2 className="text-2xl font-bold text-gray-900 mb-4">
          Ограниченный доступ к проекту
        </h2>
        
        <p className="text-gray-600 mb-6">
          Вы просматриваете проект <strong>"{projectTitle}"</strong>, но не являетесь его участником. 
          Для полного доступа к функциям проекта (канбан доска, чат, управление задачами) 
          необходимо присоединиться к проекту.
        </p>
        
        <div className="bg-blue-50 border border-blue-200 rounded-xl p-4 mb-6">
          <div className="flex items-center mb-2">
            <Users className="h-5 w-5 text-blue-600 mr-2" />
            <h3 className="font-semibold text-blue-900">Что доступно:</h3>
          </div>
          <ul className="text-sm text-blue-800 space-y-1">
            <li>• Просмотр информации о проекте</li>
            <li>• Просмотр участников проекта</li>
            <li>• Просмотр вакансий и подача заявок</li>
          </ul>
        </div>
        
        <div className="bg-orange-50 border border-orange-200 rounded-xl p-4 mb-6">
          <div className="flex items-center mb-2">
            <AlertCircle className="h-5 w-5 text-orange-600 mr-2" />
            <h3 className="font-semibold text-orange-900">Что недоступно:</h3>
          </div>
          <ul className="text-sm text-orange-800 space-y-1">
            <li>• Канбан доска и управление задачами</li>
            <li>• Чат проекта</li>
            <li>• Создание и редактирование задач</li>
          </ul>
        </div>
        
        <div className="flex flex-col sm:flex-row gap-3 justify-center">
          <Link
            to="/projects"
            className="px-6 py-3 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition-colors font-medium"
          >
            Вернуться к проектам
          </Link>
          <Link
            to={`/project/${projectId}/get`}
            className="px-6 py-3 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors font-medium"
          >
            Просмотр проекта
          </Link>
        </div>
      </div>
    </div>
  );
}; 