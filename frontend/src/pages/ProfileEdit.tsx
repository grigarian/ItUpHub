import React, { useState } from 'react';
import axios from 'axios';
import { toast } from 'react-hot-toast';

type UserProfile = {
  bio: string;
  picture: string;
};

type ProfileEditProps = {
  initialProfile: UserProfile;
  onSave: (updatedProfile: UserProfile) => void;
  onCancel: () => void;
};

export default function ProfileEdit({ initialProfile, onSave, onCancel }: ProfileEditProps) {
  const [form, setForm] = useState<UserProfile>(initialProfile);
  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      await axios.put('/user/profile', form, { withCredentials: true });
      toast.success('Профиль обновлен');
      onSave(form); // Передаем обновленные данные наверх
    } catch (error) {
      toast.error('Ошибка при обновлении профиля');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="max-w-xl mx-auto p-4 bg-white rounded shadow space-y-4">
      <div>
        <label htmlFor="bio" className="block mb-1 font-medium">О себе</label>
        <textarea
          id="bio"
          name="bio"
          value={form.bio}
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
          value={form.picture}
          onChange={handleChange}
          className="w-full border px-3 py-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <div className="flex justify-between">
        <button
          type="button"
          onClick={onCancel}
          disabled={loading}
          className="px-4 py-2 rounded border border-gray-300 hover:bg-gray-100 transition"
        >
          Отмена
        </button>

        <button
          type="submit"
          disabled={loading}
          className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition disabled:opacity-50"
        >
          {loading ? 'Сохраняем...' : 'Сохранить'}
        </button>
      </div>
    </form>
  );
}