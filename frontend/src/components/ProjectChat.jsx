import React, { useEffect, useState, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import api from '../api/axios';
import { Link } from 'react-router-dom';

export const ProjectChat = ({ projectId }) => {
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState('');
  const connectionRef = useRef(null);
  const messagesEndRef = useRef(null);
  const inputRef = useRef(null);

  useEffect(() => {
    api.get(`projectmessage/${projectId}/messages`, { withCredentials: true })
      .then(res => {
        if (Array.isArray(res.data)) {
          setMessages(res.data.reverse());
        } else {
          console.error('Некорректный формат сообщений:', res.data);
          setMessages([]);
        }
      })
      .catch(console.error);

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:8081/hubs/projectMessage?projectId=${projectId}`)
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
  }, [projectId]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  useEffect(() => {
    inputRef.current?.focus();
  }, []);

  const sendMessage = async () => {
    if (!newMessage.trim()) return;

    try {
      await connectionRef.current.invoke('SendMessage', projectId, newMessage);
      setNewMessage('');
    } catch (err) {
      console.error('Send error:', err);
    }
  };

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