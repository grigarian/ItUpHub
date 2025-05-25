import { useEffect, useState } from 'react';
import axios from 'axios';
import { useAuth } from '../context/AuthContext';
import { toast } from 'react-hot-toast';

type Skill = {
  id: { value: string };
  title: { value: string };
};

export default function AllSkills() {
  const { user, updateUser } = useAuth();
  const [skills, setSkills] = useState<Skill[]>([]);
  const [search, setSearch] = useState('');
  const { refreshUser } = useAuth();

  useEffect(() => {
    axios.get('skill/all', { withCredentials: true })
      .then(res => setSkills(res.data))
      .catch(() => toast.error('Не удалось загрузить скиллы'));
  }, []);

  const handleAddSkill = async (skillId: string) => {
    try {
      await axios.post(`/user/${user?.id}/skill`, { skillId }, {
        withCredentials: true
      });
      toast.success('Скилл добавлен');
      await refreshUser();
    } catch {
      toast.error('Ошибка добавления скилла');
    }
  };

  const filtered = skills.filter(skill =>
    skill.title.value.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="max-w-3xl mx-auto p-6">
      <input
        type="text"
        placeholder="Поиск навыка..."
        className="w-full mb-4 p-2 border rounded"
        value={search}
        onChange={e => setSearch(e.target.value)}
      />

      <div className="flex flex-wrap gap-2">
        {filtered.map(skill => (
          <div
            key={skill.id.value}
            className="px-3 py-1 bg-gray-100 rounded-full flex items-center gap-2"
          >
            <span>{skill.title.value}</span>
            <button
              onClick={() => handleAddSkill(skill.id.value)}
              className="text-blue-600 hover:underline text-sm"
            >
              Добавить
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}