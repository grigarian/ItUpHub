import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-hot-toast';
import { Loader2 } from 'lucide-react';
import axios from 'axios';
import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

type Notification = {
  id: string;
  message: string;
  createdAt: string;
  isRead: boolean;
};

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [isLoggingOut, setIsLoggingOut] = useState(false);
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [showNotifications, setShowNotifications] = useState(false);
  const unreadCount = notifications.filter(n => !n.isRead).length;

  useEffect(() => {
  axios.get('/notifications')
    .then(res => {
      if (Array.isArray(res.data)) {
        setNotifications(res.data);
      } else {
        console.error('Некорректный формат уведомлений:', res.data);
        setNotifications([]);
      }
    })
    .catch(() => console.error('Ошибка загрузки уведомлений'));
}, []);
  

  useEffect(() => {
  if (!user) return; // Ждём, пока userId загрузится

  const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:8080/hubs/notifications", {
    accessTokenFactory: () => localStorage.getItem("access_token") ?? ""
  })
  .build();

  connection.start().catch(err => console.error('Ошибка подключения SignalR:', err));

  connection.on('ReceiveNotification', (message: string) => {
  const notification = {
    id: crypto.randomUUID(),
    message,
    createdAt: new Date().toISOString(),
    isRead: false
  };
  setNotifications(prev => [notification, ...prev]);
  console.log(message);
  toast(message);
});

  return () => {
    connection.stop();
  };
}, [user]); 

  const handleToggleNotifications = () => {
    setShowNotifications(prev => !prev);

    if (!showNotifications && unreadCount > 0) {
      axios.post('/notifications/mark-all-read')
        .then(() => {
          setNotifications(prev =>
            prev.map(n => ({ ...n, isRead: true }))
          );
        });
    }
  };

  const handleLogout = async () => {
    setIsLoggingOut(true);
    try {
      await logout();
      toast.success('Вы успешно вышли из системы');
      navigate('/');
    } catch (error) {
      toast.error('Ошибка при выходе. Попробуйте еще раз');
    } finally {
      setIsLoggingOut(false);
    }
  };

  return (
    <nav className="bg-white/90 backdrop-blur-lg border-b border-gray-200 shadow-sm px-6 py-3">
      <div className="max-w-7xl mx-auto flex justify-between items-center">
        <Link
          to="/"
          className="text-2xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent"
        >
          ItUpHub
        </Link>

        <div className="flex items-center space-x-6">
          {user ? (
            <>
              <Link
                to="/my-projects"
                className="text-gray-700 hover:text-indigo-600 px-3 py-2 rounded-lg font-medium transition-colors"
              >
                Мои проекты
              </Link>

              {/* Уведомления */}
              <div className="relative">
                <button
                  onClick={handleToggleNotifications}
                  className="relative p-2 rounded-full hover:bg-gray-100 transition-colors"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth={1.5}
                    stroke="currentColor"
                    className="w-6 h-6 text-gray-600"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M14.857 17.082a23.848 23.848 0 005.454-1.31A8.967 8.967 0 0118 9.75v-.7V9A6 6 0 006 9v.75a8.967 8.967 0 01-2.312 6.022c1.733.64 3.56 1.085 5.455 1.31m5.714 0a24.255 24.255 0 01-5.714 0m5.714 0a3 3 0 11-5.714 0"
                    />
                  </svg>
                  {unreadCount > 0 && (
                    <span className="absolute top-0 right-0 bg-red-500 text-white text-xs px-1.5 py-0.5 rounded-full">
                      {unreadCount}
                    </span>
                  )}
                </button>

                {showNotifications && (
                  <div className="absolute right-0 mt-2 w-80 bg-white rounded-xl shadow-lg border border-gray-100 overflow-hidden z-50 animate-scale-in">
                    <div className="p-4 border-b font-bold text-gray-800">Уведомления</div>
                    {notifications.length === 0 ? (
                      <div className="p-4 text-gray-500 text-center">Нет уведомлений</div>
                    ) : (
                      <ul className="max-h-80 overflow-y-auto divide-y divide-gray-100">
                        {notifications.map(n => (
                          <li
                            key={n.id}
                            className={`px-4 py-3 ${n.isRead ? 'bg-white' : 'bg-indigo-50'}`}
                          >
                            <p className="text-gray-800">{n.message}</p>
                            <p className="text-xs text-gray-500 mt-1">
                              {new Date(n.createdAt).toLocaleString()}
                            </p>
                          </li>
                        ))}
                      </ul>
                    )}
                  </div>
                )}
              </div>

              {/* Профиль */}
              <Link
                to="/profile"
                className="flex items-center space-x-2 group"
              >
                {user.avatar ? (
                  <img
                    src={`/user/images/${user.avatar}`}
                    alt="Аватар"
                    className="w-9 h-9 rounded-full border-2 border-white shadow object-cover"
                  />
                ) : (
                  <div className="w-9 h-9 rounded-full bg-gradient-to-r from-indigo-500 to-purple-500 text-white flex items-center justify-center font-bold shadow">
                    {user.userName[0].toUpperCase()}
                  </div>
                )}
                <span className="font-medium text-gray-700 group-hover:text-indigo-600 transition-colors">
                  {user.userName}
                </span>
              </Link>

              {/* Кнопка выхода */}
              <button
                onClick={handleLogout}
                disabled={isLoggingOut}
                className="px-4 py-2 bg-red-600 hover:bg-red-700 text-white font-medium rounded-lg transition-colors shadow hover:shadow-red-200 flex items-center gap-2 disabled:opacity-70"
              >
                {isLoggingOut ? (
                  <>
                    <Loader2 className="w-4 h-4 animate-spin" />
                    Выход...
                  </>
                ) : (
                  'Выйти'
                )}
              </button>
            </>
          ) : (
            <div className="flex items-center gap-4">
              <Link
                to="/login"
                className="px-4 py-2 text-gray-700 hover:text-indigo-600 font-medium transition-colors"
              >
                Вход
              </Link>
              <Link
                to="/register"
                className="px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white font-medium rounded-lg transition-colors shadow hover:shadow-indigo-200"
              >
                Регистрация
              </Link>
            </div>
          )}
        </div>
      </div>
    </nav>
  );
}