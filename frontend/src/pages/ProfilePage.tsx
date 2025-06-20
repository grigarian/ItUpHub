import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import api from '../api/axios';
import { toast } from 'react-hot-toast';
import { Link } from 'react-router-dom';
import { Loader2 } from 'lucide-react';

type Skill = {
  id: { value: string };
  title: { value: string };
};

export default function ProfilePage() {
  const { user, logout, updateUser, refreshUser } = useAuth();
  const [isUploading, setIsUploading] = useState(false);
  const [bio, setBio] = useState(user?.bio || '');
  const [tempPicture, setTempPicture] = useState(user?.avatar || '');
  const [showSkillModal, setShowSkillModal] = useState(false);
  const [allSkills, setAllSkills] = useState<Skill[]>([]);
  const [search, setSearch] = useState('');

  const hasSkills = (user?.skills?.length ?? 0) > 0;

  useEffect(() => {
    setBio(user?.bio || '');
  }, [user?.bio]);

  useEffect(() => {
    if (showSkillModal) {
      api
        .get('/skill/all', { withCredentials: true })
        .then(res => setAllSkills(res.data))
        .catch(() => toast.error('Не удалось загрузить скиллы'));
    }
  }, [showSkillModal]);

  const handleImageUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file || !user) return;

    try {
      setIsUploading(true);
      const formData = new FormData();
      formData.append('file', file);

      const { data } = await api.post(
        `user/${user.id}/profile-image`,
        formData,
        {
          headers: { 'Content-Type': 'multipart/form-data' },
          withCredentials: true
        }
      );

      setTempPicture(data.picturePath);
      await refreshUser();
      toast.success('Аватар обновлён!');
    } catch {
      toast.error('Ошибка загрузки изображения');
    } finally {
      setIsUploading(false);
    }
  };

  const handleSaveBio = async () => {
    try {
      await api.put(`/user/${user?.id}/bio`, { bio }, { withCredentials: true });
      await refreshUser();
      toast.success('Биография обновлена');
    } catch {
      toast.error('Ошибка сохранения');
    }
  };

  const handleRemoveSkill = async (skillId: string) => {
    if (!user) return;

    try {
      await api.delete(`/user/${user.id}/skills/${skillId}`, {
        withCredentials: true
      });
      toast.success('Навык удалён');
      await refreshUser();
    } catch {
      toast.error('Ошибка при удалении навыка');
    }
  };

  const handleAddSkill = async (skillId: string) => {
    try {
      await api.post(`/user/${user?.id}/skill`, { skillId }, { withCredentials: true });
      toast.success('Навык добавлен');
      await refreshUser();
    } catch {
      toast.error('Ошибка добавления навыка');
    }
  };

  const filteredSkills = allSkills.filter(skill =>
    skill.title.value.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="max-w-4xl mx-auto mt-8 p-6 bg-white/90 backdrop-blur-lg rounded-3xl shadow-[0_8px_30px_rgb(0,0,0,0.12)] space-y-8 border border-gray-100">
      <div className="flex flex-col md:flex-row gap-8">
        {/* Аватар */}
        <div className="flex-shrink-0">
          <div className="relative group">
            <label className="cursor-pointer block">
              <div className="w-40 h-40 rounded-full border-4 border-white shadow-lg overflow-hidden relative">
                <img
                  src={`/api/user/images/${user?.avatar}`}
                  alt="Аватар"
                  className="w-full h-full object-cover"
                />
                {isUploading && (
                  <div className="absolute inset-0 bg-black/50 flex items-center justify-center">
                    <Loader2 className="animate-spin w-8 h-8 text-white" />
                  </div>
                )}
              </div>
              <div className="absolute inset-0 rounded-full opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center bg-black/30">
                <span className="text-white font-medium text-sm">Изменить</span>
              </div>
              <input
                type="file"
                accept="image/*"
                onChange={handleImageUpload}
                className="hidden"
                disabled={isUploading}
              />
            </label>
          </div>
        </div>

        {/* Информация профиля */}
        <div className="flex-1 space-y-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">{user?.userName}</h1>
          </div>

          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">О себе</label>
              <textarea
                value={bio}
                onChange={(e) => setBio(e.target.value)}
                className="w-full p-4 border border-gray-200 rounded-xl focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                rows={4}
                placeholder="Расскажите о себе..."
              />
              <button
                onClick={handleSaveBio}
                className="mt-3 px-5 py-2.5 bg-indigo-600 text-white font-medium rounded-lg hover:bg-indigo-700 transition-colors shadow-md hover:shadow-indigo-200"
              >
                Сохранить
              </button>
            </div>

            {/* Навыки */}
            <div>
              <div className="flex items-center justify-between mb-3">
                <h2 className="text-xl font-bold text-gray-900">Навыки</h2>
                <button
                  onClick={() => setShowSkillModal(true)}
                  className="px-4 py-2 bg-white border border-indigo-600 text-indigo-600 font-medium rounded-lg hover:bg-indigo-50 transition-colors"
                >
                  Добавить навык
                </button>
              </div>

              {hasSkills ? (
                <div className="flex flex-wrap gap-2">
                  {user?.skills.map((skill: any) => (
                    <div
                      key={skill.id.value}
                      className="relative group bg-indigo-100 text-indigo-800 px-4 py-2 rounded-full flex items-center hover:bg-indigo-200 transition-colors"
                    >
                      {skill.title.value}
                      <button
                        onClick={() => handleRemoveSkill(skill.id.value)}
                        className="ml-2 text-indigo-600 hover:text-indigo-800 opacity-0 group-hover:opacity-100 transition-opacity"
                        title="Удалить навык"
                      >
                        ×
                      </button>
                    </div>
                  ))}
                </div>
              ) : (
                <div className="p-4 bg-gray-50 rounded-xl text-center">
                  <p className="text-gray-500">
                    У вас пока нет добавленных навыков.{' '}
                    <button
                      onClick={() => setShowSkillModal(true)}
                      className="text-indigo-600 hover:underline font-medium"
                    >
                      Добавить сейчас
                    </button>
                  </p>
                </div>
              )}
            </div>

            <button
              onClick={logout}
              className="px-5 py-2.5 bg-red-600 text-white font-medium rounded-lg hover:bg-red-700 transition-colors shadow-md hover:shadow-red-200"
            >
              Выйти из аккаунта
            </button>
          </div>
        </div>
      </div>

      {/* Модалка выбора навыков */}
      {showSkillModal && (
        <div className="fixed inset-0 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm z-50 animate-fade-in">
          <div className="w-full max-w-xl bg-white rounded-2xl shadow-xl overflow-hidden transform transition-all duration-300 ease-out animate-scale-in">
            <div className="p-6">
              <div className="flex justify-between items-center mb-4">
                <h2 className="text-2xl font-bold text-gray-900">Добавить навыки</h2>
                <button
                  onClick={() => setShowSkillModal(false)}
                  className="text-gray-500 hover:text-gray-700 text-2xl"
                >
                  &times;
                </button>
              </div>

              <input
                type="text"
                placeholder="Поиск навыка..."
                className="w-full p-4 border border-gray-200 rounded-xl focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all mb-4"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
              />

              <div className="max-h-96 overflow-y-auto pr-2">
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                  {filteredSkills.map(skill => (
                    <div
                      key={skill.id.value}
                      className="p-3 bg-gray-50 rounded-lg border border-gray-100 hover:bg-gray-100 transition-colors flex justify-between items-center"
                    >
                      <span className="font-medium text-gray-800">{skill.title.value}</span>
                      <button
                        onClick={() => handleAddSkill(skill.id.value)}
                        className="px-3 py-1 bg-indigo-600 text-white text-sm font-medium rounded-lg hover:bg-indigo-700 transition-colors"
                      >
                        Добавить
                      </button>
                    </div>
                  ))}
                </div>
              </div>
            </div>
            
            <div className="px-6 py-4 bg-gray-50 flex justify-end">
              <button
                onClick={() => setShowSkillModal(false)}
                className="px-5 py-2.5 bg-white border border-gray-300 text-gray-700 font-medium rounded-lg hover:bg-gray-50 transition-colors"
              >
                Закрыть
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}