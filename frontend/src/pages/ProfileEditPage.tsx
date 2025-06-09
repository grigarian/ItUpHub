import React, { useEffect, useState } from 'react';
import api from '../api/axios';
import { toast } from 'react-hot-toast';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

type UserProfile = {
  bio: string;
  picture: string;
};

export default function ProfileEditPage() {
  const { user } = useAuth();
  const navigate = useNavigate();

  const [profile, setProfile] = useState<UserProfile>({ bio: '', picture: '' });
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!user?.id) {
      toast.error('Пользователь не авторизован');
      navigate('/login');
      return;
    }

    const fetchProfile = async () => {
      try {
        const res = await api.get(`/user/profile/${user.id}`, { withCredentials: true });
        setProfile({
          bio: res.data.bio || '',
          picture: res.data.picture || '',
        });
      } catch (error) {
        toast.error('Ошибка загрузки профиля');
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, [user, navigate]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setProfile(prev => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!user?.id) {
      toast.error('Пользователь не авторизован');
      navigate('/login');
      return;
    }

    setSaving(true);

    try {
      await api.put('/user/profile', profile, { withCredentials: true });
      toast.success('Профиль успешно обновлён');
      navigate('/profile');
    } catch (error) {
      toast.error('Ошибка при обновлении профиля');
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <div>Загрузка...</div>;

  return (
    <form onSubmit={handleSubmit} className="max-w-xl mx-auto p-4 bg-white rounded shadow space-y-4">
      <div>
        <label htmlFor="bio" className="block mb-1 font-medium">О себе</label>
        <textarea
          id="bio"
          name="bio"
          value={profile.bio}
          onChange={handleChange}
          rows={4}
          className="w-full border px-3 py-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <div>
        <label htmlFor="picture" className="block mb-1 font-medium">Ссылка на изображение</label>
        <input
          id="picture"
          name="picture"
          type="text"
          value={profile.picture}
          onChange={handleChange}
          className="w-full border px-3 py-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <div className="flex justify-between">
        <button
          type="button"
          onClick={() => navigate('/profile')}
          disabled={saving}
          className="px-4 py-2 rounded border border-gray-300 hover:bg-gray-100 transition"
        >
          Отмена
        </button>

        <button
          type="submit"
          disabled={saving}
          className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition disabled:opacity-50"
        >
          {saving ? 'Сохраняем...' : 'Сохранить'}
        </button>
      </div>
    </form>
  );
}