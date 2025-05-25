import { useEffect, useState } from 'react';
import axios from 'axios';

type Notification = {
  id: string;
  message: string;
  createdAt: string;
  isRead: boolean;
};

const Notifications = () => {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [showNotifications, setShowNotifications] = useState(false);
  const unreadCount = notifications.filter(n => !n.isRead).length;

  useEffect(() => {
    axios.get('/notifications')
      .then(res => setNotifications(res.data))
      .catch(() => console.error('Ошибка загрузки уведомлений'));
  }, []);

  const handleToggleNotifications = () => {
    setShowNotifications(prev => !prev);

    // Отметить все как прочитанные при открытии
    if (!showNotifications && unreadCount > 0) {
      axios.post('/notifications/mark-all-read')
        .then(() => {
          setNotifications(prev =>
            prev.map(n => ({ ...n, isRead: true }))
          );
        });
    }
  };

  return (
    <div className="relative">
      <button
        onClick={handleToggleNotifications}
        className="relative text-2xl"
      >
        🔔
        {unreadCount > 0 && (
          <span className="absolute -top-1 -right-1 bg-red-600 text-white text-xs px-1.5 py-0.5 rounded-full">
            {unreadCount}
          </span>
        )}
      </button>

      {showNotifications && (
        <div className="absolute right-0 mt-2 w-80 bg-white border shadow-lg rounded z-50">
          <div className="p-4 border-b font-semibold">Уведомления</div>
          {notifications.length === 0 ? (
            <div className="p-4 text-gray-500">Нет уведомлений</div>
          ) : (
            <ul className="max-h-80 overflow-y-auto">
              {notifications.map(n => (
                <li
                  key={n.id}
                  className={`px-4 py-2 border-b ${
                    n.isRead ? 'bg-white' : 'bg-blue-50 font-medium'
                  }`}
                >
                  <p>{n.message}</p>
                  <small className="text-gray-400">{new Date(n.createdAt).toLocaleString()}</small>
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
};

export default Notifications;