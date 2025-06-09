import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import api from '../api/axios';
import { Loader2 } from 'lucide-react';
import toast from 'react-hot-toast';
import { ProjectAccessInfo } from './ProjectAccessInfo';

interface ProjectMembershipCheckProps {
  children: React.ReactNode;
}

export const ProjectMembershipCheck: React.FC<ProjectMembershipCheckProps> = ({ children }) => {
  const { projectId } = useParams();
  const { user } = useAuth();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [isMember, setIsMember] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);
  const [projectTitle, setProjectTitle] = useState('');

  useEffect(() => {
    const checkMembership = async () => {
      if (!user || !projectId) {
        setIsLoading(false);
        return;
      }

      try {
        // Проверяем, является ли пользователь админом
        if (user.isAdmin) {
          setIsAdmin(true);
          setIsMember(true);
          setIsLoading(false);
          return;
        }

        // Получаем список членов проекта
        const response = await api.get(`/project/${projectId}/members`, { 
          withCredentials: true 
        });
        const members = response.data;
        const userIsMember = members.some((member: any) => member.userId === user.id);
        
        setIsMember(userIsMember);
        
        // Получаем название проекта для отображения в сообщении
        if (!userIsMember) {
          try {
            const projectResponse = await api.get(`/project/${projectId}/get`);
            setProjectTitle(projectResponse.data.title);
          } catch (error) {
            console.error('Error fetching project title:', error);
            setProjectTitle('Проект');
          }
        }
      } catch (error) {
        console.error('Error checking membership:', error);
        toast.error('Ошибка при проверке доступа к проекту');
        navigate('/projects');
      } finally {
        setIsLoading(false);
      }
    };

    checkMembership();
  }, [projectId, user, navigate]);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <Loader2 className="animate-spin h-12 w-12 text-indigo-600 mx-auto mb-4" />
          <p className="text-gray-600">Проверка доступа к проекту...</p>
        </div>
      </div>
    );
  }

  if (!isMember && !isAdmin) {
    return (
      <ProjectAccessInfo 
        projectId={projectId!} 
        projectTitle={projectTitle} 
      />
    );
  }

  return <>{children}</>;
}; 