import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import api from '../api/axios';
import { toast } from 'react-hot-toast';
import { useAuth } from '../context/AuthContext';
import { Loader2 } from 'lucide-react';
import CreateVacancyModal from '../components/CreateVacancyModal';
import VacancyApplicationsModal from '../components/VacancyApplicationsModal';
import { useSEO } from '../utils/hooks/useSEO';
import { SEO_CONFIGS } from '../utils/seo';
import {
  Project,
  Member,
  JoinRequestDto,
  Skill,
  SkillDto,
  ProjectVacancyDto,
  VacancyApplicationDto,
  roleTranslations,
  statusTranslations
} from '../types';

// Функция для форматирования даты
const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('ru-RU', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
};

// Функция для форматирования даты и времени
const formatDateTime = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleString('ru-RU', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

export default function ProjectPage() {
  const { id } = useParams();
  const [project, setProject] = useState<Project | null>(null);
  const [loading, setLoading] = useState(true);
  const { user } = useAuth();
  const [activeTab, setActiveTab] = useState('overview');
  const [vacancies, setVacancies] = useState<ProjectVacancyDto[]>([]);
  const [loadingVacancies, setLoadingVacancies] = useState(false);
  const [showCreateVacancyModal, setShowCreateVacancyModal] = useState(false);
  const [showApplicationsModal, setShowApplicationsModal] = useState(false);
  const [selectedVacancy, setSelectedVacancy] = useState<ProjectVacancyDto | null>(null);
  const [allSkills, setAllSkills] = useState<Skill[]>([]);

  const isMember = project?.members.some(m => m.userId === user?.id);
  const currentUserRole = project?.members.find(m => m.userId === user?.id)?.role;
  const isManager = currentUserRole === 'ProjectManager';

  // SEO оптимизация для страницы проекта
  useEffect(() => {
    if (project) {
      useSEO(SEO_CONFIGS.project(project.title, project.description));
    }
  }, [project]);

  useEffect(() => {
    const fetchProject = async () => {
      try {
        const response = await api.get(`/project/${id}/get`);
        setProject(response.data);
      } catch {
        toast.error('Ошибка при загрузке проекта');
      } finally {
        setLoading(false);
      }
    };

    fetchProject();
  }, [id]);

  useEffect(() => {
    const fetchSkills = async () => {
      try {
        const response = await api.get('/skill/all', { withCredentials: true });
        setAllSkills(response.data);
      } catch {
        toast.error('Не удалось загрузить скиллы');
      }
    };

    fetchSkills();
  }, []);

  const fetchVacancies = async () => {
    if (!project) return;
    
    setLoadingVacancies(true);
    try {
      const response = await api.get(`/projectvacancies/${project.id}`);
      setVacancies(response.data);
    } catch {
      toast.error('Не удалось загрузить вакансии');
    } finally {
      setLoadingVacancies(false);
    }
  };

  useEffect(() => {
    if (activeTab === 'vacancies' && project) {
      fetchVacancies();
    }
  }, [activeTab, project]);

  const handleCreateVacancy = async (title: string, description: string, skillIds: string[]) => {
    if (!project) return;

    try {
      await api.post(`/projectvacancies/${project.id}`, {
        Title: title,
        Description: description,
        SkillIds: skillIds
      });
      toast.success('Вакансия создана');
      fetchVacancies();
    } catch {
      toast.error('Ошибка при создании вакансии');
    }
  };

  const handleApplyToVacancy = async (vacancyId: string, message: string) => {
    try {
      await api.post(`/projectvacancies/${vacancyId}/apply`, { Message: message });
      toast.success('Заявка подана');
    } catch {
      toast.error('Ошибка при подаче заявки');
    }
  };

  const handleApproveApplication = async (applicationId: string, message: string, role: string) => {
    try {
      await api.post(`/projectvacancies/${id}/application/${applicationId}/approve`, { Message: message, Role: role });
      toast.success('Заявка принята');
      if (selectedVacancy) {
        setSelectedVacancy({
          ...selectedVacancy,
          applications: selectedVacancy.applications.filter(app => app.id !== applicationId)
        });
      }
    } catch {
      toast.error('Ошибка при принятии заявки');
    }
  };

  const handleRejectApplication = async (applicationId: string, message: string) => {
    try {
      await api.post(`/project-vacancies/application/${applicationId}/reject`, { Message: message });
      toast.success('Заявка отклонена');
      if (selectedVacancy) {
        setSelectedVacancy({
          ...selectedVacancy,
          applications: selectedVacancy.applications.filter(app => app.id !== applicationId)
        });
      }
    } catch {
      toast.error('Ошибка при отклонении заявки');
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-60">
        <Loader2 className="animate-spin w-10 h-10 text-indigo-600" />
      </div>
    );
  }

  if (!project) {
    return <p className="text-center mt-10 text-xl text-gray-700">Проект не найден</p>;
  }

  return (
    <div className="max-w-6xl mx-auto mt-8 p-6 bg-white/90 backdrop-blur-lg rounded-3xl shadow-lg space-y-8 border border-gray-100">
      {/* Заголовок проекта */}
      <div className="space-y-4">
        <div className="flex justify-between items-start">
          <h1 className="text-4xl font-bold text-gray-900 bg-clip-text bg-gradient-to-r from-indigo-600 to-purple-600">
            {project.title}
          </h1>
          <div className="flex items-center gap-4">
            <span className="px-3 py-1 rounded-full text-sm font-medium bg-indigo-100 text-indigo-800">
              {statusTranslations[project.status] ?? project.status}
            </span>
            {(isMember || user?.isAdmin) && (
              <Link
                to={`/projects/${project.id}/board`}
                className="px-4 py-2 rounded-lg bg-indigo-600 text-white hover:bg-indigo-700 transition text-sm font-medium"
              >
                Доска задач
              </Link>
            )}
          </div>
        </div>
        
        {/* Навигационные вкладки */}
        <div className="border-b border-gray-200">
          <nav className="flex space-x-8">
            {['overview', 'members', 'vacancies'].map((tab) => (
              <button
                key={tab}
                onClick={() => setActiveTab(tab)}
                className={`py-4 px-1 text-sm font-medium ${
                  activeTab === tab
                    ? 'border-b-2 border-indigo-500 text-indigo-600'
                    : 'text-gray-500 hover:text-gray-700 hover:border-gray-300'
                }`}
              >
                {tab === 'overview' && 'Обзор'}
                {tab === 'members' && 'Участники'}
                {tab === 'vacancies' && 'Вакансии'}
              </button>
            ))}
          </nav>
        </div>

        {/* Контент вкладок */}
        {activeTab === 'overview' && (
          <>
            <p className="text-lg text-gray-700">{project.description}</p>
            <div className="grid md:grid-cols-3 gap-4 mt-6">
              <div className="p-4 bg-gray-50 rounded-xl">
                <h3 className="text-sm text-gray-500">Категория</h3>
                <p className="mt-1 text-lg font-medium">{project.categoryName}</p>
              </div>
              <div className="p-4 bg-gray-50 rounded-xl">
                <h3 className="text-sm text-gray-500">Дата начала</h3>
                <p className="mt-1 text-lg font-medium">{formatDate(project.startDate)}</p>
              </div>
              <div className="p-4 bg-gray-50 rounded-xl">
                <h3 className="text-sm text-gray-500">Дата окончания</h3>
                <p className="mt-1 text-lg font-medium">{project.endDate ? formatDate(project.endDate) : 'Не указана'}</p>
              </div>
            </div>
            <div className="grid md:grid-cols-2 gap-4 mt-4">
              <div className="p-4 bg-gray-50 rounded-xl">
                <h3 className="text-sm text-gray-500">Дата создания</h3>
                <p className="mt-1 text-lg font-medium">{formatDateTime(project.creationDate)}</p>
              </div>
              <div className="p-4 bg-gray-50 rounded-xl">
                <h3 className="text-sm text-gray-500">Статус</h3>
                <p className="mt-1 text-lg font-medium">{statusTranslations[project.status] ?? project.status}</p>
              </div>
            </div>
          </>
        )}

        {activeTab === 'members' && (
          <section>
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-2xl font-bold text-gray-900">Участники проекта</h2>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              {project.members.map(member => (
            <div key={member.userId} className="flex items-center p-4 bg-white rounded-xl border hover:shadow-md">
              <div className="relative">
                {member.pictureUrl ? (
                  <img
                    src={`/api/user/images/${member.pictureUrl}`}
                    alt={member.userName}
                    className="w-14 h-14 rounded-full object-cover"
                  />
                ) : (
                  <div className="w-14 h-14 rounded-full bg-gradient-to-r from-indigo-500 to-purple-500 text-white flex items-center justify-center font-bold text-xl">
                    {member.userName.charAt(0).toUpperCase()}
                  </div>
                )}
                <span className="absolute -bottom-1 -right-1 bg-white px-2 py-0.5 rounded-full text-xs border">
                  {roleTranslations[member.role] || member.role}
                </span>
              </div>
              <div className="ml-4">
                <Link
                  to={`/user/${member.userId}`}
                  className="text-lg font-medium text-gray-900 hover:text-indigo-600"
                >
                  {member.userName}
                </Link>
              </div>
            </div>
          ))}
            </div>
          </section>
        )}

        {activeTab === 'vacancies' && (
          <section>
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-2xl font-bold text-gray-900">Вакансии проекта</h2>
              {isManager && (
                <button
                  onClick={() => setShowCreateVacancyModal(true)}
                  className="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700"
                >
                  Создать вакансию
                </button>
              )}
            </div>

            {loadingVacancies ? (
              <div className="flex justify-center py-10">
                <Loader2 className="animate-spin w-8 h-8 text-indigo-600" />
              </div>
            ) : vacancies.length === 0 ? (
              <div className="text-center py-10 bg-gray-50 rounded-xl">
                <p className="text-gray-500">Нет активных вакансий</p>
              </div>
            ) : (
              <div className="space-y-6">
                {vacancies.map(vacancy => (
                  <div key={vacancy.id} className="p-6 bg-white rounded-xl border border-gray-200">
                    <div className="flex justify-between items-start">
                      <div>
                        <h3 className="text-xl font-semibold text-gray-900">{vacancy.title}</h3>
                        <p className="mt-2 text-gray-600">{vacancy.description}</p>
                        <div className="mt-3 flex flex-wrap gap-2">
                          {vacancy.skills.map(skill => (
                            <span 
                              key={skill.id} 
                              className="px-3 py-1 bg-indigo-100 text-indigo-800 rounded-full text-sm"
                            >
                              {skill.title}
                            </span>
                          ))}
                        </div>
                      </div>
                      <div className="flex flex-col items-end space-y-2">
                        <span className="text-sm text-gray-500">
                          {formatDateTime(vacancy.createdAt)}
                        </span>
                        {isManager && vacancy.applications.length > 0 && (
                          <button
                            onClick={() => {
                              setSelectedVacancy(vacancy);
                              setShowApplicationsModal(true);
                            }}
                            className="px-4 py-2 bg-indigo-100 text-indigo-700 rounded-lg hover:bg-indigo-200"
                          >
                            Заявки ({vacancy.applications.length})
                          </button>
                        )}
                      </div>
                    </div>
                    {!isMember && (
                      <button
                        onClick={() => {
                          setSelectedVacancy(vacancy);
                          setShowApplicationsModal(true);
                        }}
                        className="mt-4 px-4 py-2 bg-green-100 text-green-700 rounded-lg hover:bg-green-200"
                      >
                        Подать заявку
                      </button>
                    )}
                  </div>
                ))}
              </div>
            )}
          </section>
        )}
      </div>

      {/* Модальное окно создания вакансии */}
      <CreateVacancyModal
        isOpen={showCreateVacancyModal}
        onClose={() => setShowCreateVacancyModal(false)}
        onCreate={handleCreateVacancy}
        allSkills={allSkills}
      />

      {/* Модальное окно заявок на вакансию */}
      <VacancyApplicationsModal
        isOpen={showApplicationsModal}
        onClose={() => setShowApplicationsModal(false)}
        vacancy={selectedVacancy}
        isManager={isManager}
        currentUserId={user?.id || ''}
        onApply={handleApplyToVacancy}
        onApprove={handleApproveApplication}
        onReject={handleRejectApplication}
      />
    </div>
  );
}