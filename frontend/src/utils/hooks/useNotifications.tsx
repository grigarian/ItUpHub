import { useEffect, useState, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import toast from 'react-hot-toast';
import { getCookie } from '../cookies';

export function useNotificationHub(userId: string | null, accessToken: string | null) {
  const [notifications, setNotifications] = useState<string[]>([]);
  const connection = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    if (!userId || !accessToken) return; // ждем и userId, и токен

    connection.current = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:8080/hubs/notifications", {
    accessTokenFactory: () => getCookie('tasty-cookies') ?? ""
  })
  .build();

    connection.current
      .start()
      .then(() => {
        console.log('SignalR connected');

        connection.current?.on('ReceiveNotification', (message: string) => {
          setNotifications(prev => [...prev, message]);
          console.log("Получено уведомление:", message);
          toast.success(message);
        });

        // Если нужно: регистрация пользователя на сервере
        // connection.current.invoke('RegisterUser', userId).catch(console.error);
      })
      .catch(err => console.error('Ошибка подключения SignalR:', err));

    return () => {
      connection.current?.stop();
    };
  }, [userId, accessToken]);

  return notifications;
}