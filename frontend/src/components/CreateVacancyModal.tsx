import { useState } from 'react';
import { Skill } from '../types';

type CreateVacancyModalProps = {
  isOpen: boolean;
  onClose: () => void;
  onCreate: (title: string, description: string, skillIds: string[]) => void;
  allSkills: Skill[];
};

export default function CreateVacancyModal({ 
  isOpen, 
  onClose, 
  onCreate,
  allSkills
}: CreateVacancyModalProps) {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [selectedSkills, setSelectedSkills] = useState<string[]>([]);
  const [skillSearch, setSkillSearch] = useState('');

  const handleSubmit = () => {
    if (!title.trim()) return;
    onCreate(title, description, selectedSkills);
    setTitle('');
    setDescription('');
    setSelectedSkills([]);
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
      <div className="bg-white rounded-xl p-6 w-full max-w-md shadow-lg">
        <h2 className="text-xl font-bold mb-4">Создать вакансию</h2>
        
        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Название</label>
            <input
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className="w-full border rounded-lg p-2"
              placeholder="Введите название вакансии"
            />
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Описание</label>
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="w-full border rounded-lg p-2 h-32"
              placeholder="Опишите вакансию"
            />
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Требуемые навыки</label>
            <input
              type="text"
              value={skillSearch}
              onChange={(e) => setSkillSearch(e.target.value)}
              className="w-full border rounded-lg p-2 mb-2"
              placeholder="Поиск навыков"
            />
            <div className="max-h-40 overflow-y-auto border rounded-lg p-2">
              {allSkills
                .filter(skill => 
                  skill.title.value.toLowerCase().includes(skillSearch.toLowerCase())
                )
                .map(skill => (
                  <div key={skill.id.value} className="flex items-center py-1">
                    <input
                      type="checkbox"
                      checked={selectedSkills.includes(skill.id.value)}
                      onChange={() => {
                        setSelectedSkills(prev => 
                          prev.includes(skill.id.value)
                            ? prev.filter(id => id !== skill.id.value)
                            : [...prev, skill.id.value]
                        );
                      }}
                      className="mr-2"
                    />
                    <span>{skill.title.value}</span>
                  </div>
                ))}
            </div>
          </div>
        </div>
        
        <div className="flex justify-end space-x-2 mt-6">
          <button 
            onClick={onClose}
            className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
          >
            Отмена
          </button>
          <button 
            onClick={handleSubmit}
            disabled={!title.trim()}
            className={`px-4 py-2 rounded-lg ${
              !title.trim()
                ? 'bg-gray-300 text-gray-500 cursor-not-allowed'
                : 'bg-indigo-600 text-white hover:bg-indigo-700'
            }`}
          >
            Создать
          </button>
        </div>
      </div>
    </div>
  );
}