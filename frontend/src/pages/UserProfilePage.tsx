import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from '../api/axios';
import { toast } from 'react-hot-toast';

type Skill = {
  id: { value: string };
  title: { value: string };
};

type User = {
  id: string;
  userName: string;
  email: string;
  bio: string;
  avatar: string;
  skills: Skill[];
};

export default function UserProfilePage() {
  const { userId } = useParams();
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/user/${userId}`)
      .then(res => setUser(res.data))
      .catch(() => toast.error('Не удалось загрузить пользователя'))
      .finally(() => setLoading(false));
  }, [userId]);

  if (loading) return <p className="text-center mt-10">Загрузка...</p>;
  if (!user) return <p className="text-center mt-10">Пользователь не найден</p>;

  return (
    <div className="max-w-3xl mx-auto mt-8 p-6 bg-white rounded-lg shadow-md">
      <div className="flex items-start gap-8">
        <img
          src={`/api/user/images/${user.avatar}`}
          alt="Аватар"
          className="w-40 h-40 rounded-full border-4 border-gray-200 object-cover"
        />

        <div className="flex-1 space-y-4">
          <div>
            <h2 className="text-2xl font-bold">{user.userName}</h2>
          </div>

          <div>
            <label className="block text-sm font-medium mb-1">О себе</label>
            <p className="text-gray-800 whitespace-pre-line">{user.bio}</p>
          </div>

          {user.skills.length > 0 && (
            <div>
              <h3 className="text-lg font-semibold mb-2">Навыки</h3>
              <div className="flex flex-wrap gap-2">
                {user.skills.map((skill) => (
                  <div
                    key={skill.id.value}
                    className="bg-blue-100 text-blue-800 px-3 py-1 rounded-full text-sm font-medium"
                  >
                    {skill.title.value}
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}