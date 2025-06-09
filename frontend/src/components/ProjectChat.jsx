import React, { useEffect, useState, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import api from '../api/axios';
import { Link, useNavigate } from 'react-router-dom';
import { getCookie } from '../utils/cookies';
import { useAuth } from '../context/AuthContext';
import { ProjectAccessInfo } from './ProjectAccessInfo';

export const ProjectChat = ({ projectId }) => {
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState('');
  const [isMember, setIsMember] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [projectTitle, setProjectTitle] = useState('');
  const connectionRef = useRef(null);
  const messagesEndRef = useRef(null);
  const inputRef = useRef(null);
  const { user } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const checkMembershipAndLoadMessages = async () => {
      if (!user || !projectId) {
        setIsLoading(false);
        return;
      }

      try {
        // Проверяем, является ли пользователь админом
        if (user.isAdmin) {
          setIsMember(true);
        } else {
          // Получаем список членов проекта
          const response = await api.get(`/project/${projectId}/members`, { 
            withCredentials: true 
          });
          
          const members = response.data;
          const userIsMember = members.some((member) => member.userId === user.id);
          setIsMember(userIsMember);
          
          if (!userIsMember) {
            // Получаем название проекта для отображения в сообщении
            try {
              const projectResponse = await api.get(`/project/${projectId}/get`);
              setProjectTitle(projectResponse.data.title);
            } catch (error) {
              console.error('Error fetching project title:', error);
              setProjectTitle('Проект');
            }
            return; // Не загружаем сообщения, если пользователь не участник
          }
        }

        // Загружаем сообщения только если пользователь участник
        if (isMember || user.isAdmin) {
          const res = await api.get(`projectmessage/${projectId}/messages`, { withCredentials: true });
          if (Array.isArray(res.data)) {
            setMessages(res.data.reverse());
          } else {
            console.error('Некорректный формат сообщений:', res.data);
            setMessages([]);
          }
        }
      } catch (error) {
        console.error('Error checking membership or loading messages:', error);
      } finally {
        setIsLoading(false);
      }
    };

    checkMembershipAndLoadMessages();
  }, [projectId, user]);

  useEffect(() => {
    if (!isMember && !user?.isAdmin) {
      return; // Не подключаемся к SignalR, если пользователь не участник
    }

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(process.env.NODE_ENV === 'development' 
        ? `http://localhost:8081/hubs/projectMessage?projectId=${projectId}`
        : `/hubs/projectMessage?projectId=${projectId}`, {
        accessTokenFactory: () => getCookie('tasty-cookies') ?? ""
      })
      .withAutomaticReconnect()
      .build();

    connection.on('ReceiveMessage', message => {
      setMessages(prev => [...prev, message]);
    });

    connection.start().catch(err => console.error('Connection failed:', err));
    connectionRef.current = connection;

    return () => {
      connection.stop();
    };
  }, [projectId, isMember, user]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  useEffect(() => {
    inputRef.current?.focus();
  }, []);

  const sendMessage = async () => {
    if (!newMessage.trim() || (!isMember && !user?.isAdmin)) return;

    try {
      await connectionRef.current.invoke('SendMessage', projectId, newMessage);
      setNewMessage('');
    } catch (err) {
      console.error('Send error:', err);
    }
  };

  if (isLoading) {
    return (
      <div className="mt-8 max-w-xl w-full mx-auto rounded-xl shadow-md bg-white border border-gray-200 p-8">
        <div className="text-center">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-500 mx-auto mb-4"></div>
          <p className="text-gray-600">Загрузка чата...</p>
        </div>
      </div>
    );
  }

  if (!isMember && !user?.isAdmin) {
    return (
      <div className="mt-8">
        <ProjectAccessInfo 
          projectId={projectId} 
          projectTitle={projectTitle} 
        />
      </div>
    );
  }

  return (
    <div className="mt-8 max-w-xl w-full mx-auto rounded-xl shadow-md bg-white border border-gray-200">
      <div className="p-4 border-b">
        <h2 className="text-lg font-bold text-gray-700">💬 Чат проекта</h2>
      </div>

      <div className="h-72 overflow-y-auto px-4 py-2 space-y-3">
        {messages.map((m, i) => (
          <div key={i} className="text-sm bg-gray-50 p-2 rounded shadow-sm">
            <div className="text-gray-600 mb-1">
              <Link
                to={`/user/${m.sender?.id.value}`}
                className="text-blue-600 hover:underline font-medium"
              >
                {m.sender?.name}
              </Link>
              <span className="ml-2 text-xs text-gray-400">
                {new Date(m.sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
              </span>
            </div>
            <div className="text-gray-800">{m.content}</div>
          </div>
        ))}
        <div ref={messagesEndRef} />
      </div>

      <div className="border-t p-4 flex gap-2">
        <input
          ref={inputRef}
          value={newMessage}
          onChange={e => setNewMessage(e.target.value)}
          onKeyDown={e => e.key === 'Enter' && sendMessage()}
          className="flex-1 border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring focus:ring-blue-300"
          placeholder="Введите сообщение..."
        />
        <button
          onClick={sendMessage}
          className="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 text-sm"
        >
          Отправить
        </button>
      </div>
    </div>
  );
};