import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import { toast } from 'react-hot-toast';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Loader2 } from 'lucide-react';

const roleTranslations: Record<string, string> = {
  ProjectManager: 'Руководитель проекта',
  Developer: 'Разработчик',
  Designer: 'Дизайнер',
  BuisnessAnalyst: 'Бизнес-аналитик',
  Tester: 'Тестировщик',
};

const statusTranslations: Record<string, string> = {
  InProgress: 'Активен',
  Complete: 'Завершён',
  Cancelled: 'Отменён',
};

type Member = {
  userId: string;
  userName: string;
  role: string;
  pictureUrl: string;
};

type Project = {
  id: string;
  title: string;
  description: string;
  status: string;
  startDate: string;
  endDate?: string;
  creationDate: string;
  categoryName: string;
  members: Member[];
};

type JoinRequestDto = {
  id: string;
  userId: string;
  userName: string;
  message: string;
  status: string;
  createdAt: string;
};

export default function ProjectPage() {
  const { id } = useParams();
  const [project, setProject] = useState<Project | null>(null);
  const [loading, setLoading] = useState(true);
  const { user } = useAuth();
  const isMember = project?.members.some(m => m.userId === user?.id);
  const currentUserRole = project?.members.find(m => m.userId === user?.id)?.role;
  const isManager = currentUserRole === 'ProjectManager';

  const [showModal, setShowModal] = useState(false);
  const [joinMessage, setJoinMessage] = useState('');
  const [requests, setRequests] = useState<JoinRequestDto[]>([]);
  const [requestLoading, setRequestLoading] = useState(false);
  const [showRoleModal, setShowRoleModal] = useState(false);
  const [selectedRequestId, setSelectedRequestId] = useState<string | null>(null);
  const [selectedRole, setSelectedRole] = useState('');

  const handleJoinRequest = async () => {
    try {
      await axios.post(`/join-request/${project?.id}`, { message: joinMessage });
      toast.success('Заявка отправлена');
      setShowModal(false);
      setJoinMessage('');
    } catch {
      toast.error('Ошибка при отправке заявки или заявка уже отправлена');
    }
  };

  const handleReject = async (requestId: string) => {
    try {
      await axios.post(`/join-request/${requestId}/reject`, { reason: 'Не подходим' });
      toast.success('Заявка отклонена');
      setRequests(prev => prev.filter(r => r.id !== requestId));
    } catch {
      toast.error('Ошибка при отклонении заявки');
    }
  };

  const handleConfirmRole = async () => {
    if (!selectedRequestId || !selectedRole) return;

    try {
      await axios.post(`/join-request/${selectedRequestId}/approve`, { role: selectedRole });
      toast.success('Заявка одобрена');
      setRequests(prev => prev.filter(r => r.id !== selectedRequestId));
      setShowRoleModal(false);
      setSelectedRequestId(null);
      setSelectedRole('');
    } catch {
      toast.error('Ошибка при одобрении заявки');
    }
  };

  useEffect(() => {
    if (!isManager) return;
    setRequestLoading(true);
    axios
      .get(`/join-request/${project?.id}/requests`)
      .then(res => setRequests(res.data))
      .catch(() => toast.error('Ошибка при загрузке заявок'))
      .finally(() => setRequestLoading(false));
  }, [isManager, project?.id]);

  useEffect(() => {
    axios
      .get(`/project/${id}/get`)
      .then(res => setProject(res.data))
      .catch(() => toast.error('Ошибка при загрузке проекта'))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading)
    return (
      <div className="flex justify-center items-center h-60">
        <Loader2 className="animate-spin w-10 h-10 text-indigo-600" />
      </div>
    );

  if (!project) return <p className="text-center mt-10 text-xl text-gray-700">Проект не найден</p>;

  return (
    <div className="max-w-6xl mx-auto mt-8 p-6 bg-white/90 backdrop-blur-lg rounded-3xl shadow-[0_8px_30px_rgb(0,0,0,0.12)] space-y-8 border border-gray-100">
      {/* Project Header */}
      <div className="space-y-4">
        <div className="flex justify-between items-start">
          <h1 className="text-4xl font-bold text-gray-900 bg-clip-text bg-gradient-to-r from-indigo-600 to-purple-600">
            {project.title}
          </h1>
          <span className="px-3 py-1 rounded-full text-sm font-medium bg-indigo-100 text-indigo-800">
            {statusTranslations[project.status] ?? project.status}
          </span>
        </div>

        <p className="text-lg text-gray-700 leading-relaxed break-words whitespace-normal">
          {project.description}
        </p>

        <div className="grid md:grid-cols-3 gap-4 mt-6">
          <div className="p-4 bg-gray-50 rounded-xl">
            <h3 className="text-sm font-medium text-gray-500">Категория</h3>
            <p className="mt-1 text-lg font-medium">{project.categoryName}</p>
          </div>
          <div className="p-4 bg-gray-50 rounded-xl">
            <h3 className="text-sm font-medium text-gray-500">Дата начала</h3>
            <p className="mt-1 text-lg font-medium">{project.startDate}</p>
          </div>
          <div className="p-4 bg-gray-50 rounded-xl">
            <h3 className="text-sm font-medium text-gray-500">Дата окончания</h3>
            <p className="mt-1 text-lg font-medium">{project.endDate || 'Не указана'}</p>
          </div>
        </div>
      </div>

      {/* Members Section */}
      <section className="mt-10">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-bold text-gray-900">Участники проекта</h2>
          {!isMember && (
            <button
              onClick={() => setShowModal(true)}
              className="px-6 py-2.5 bg-indigo-600 text-white font-medium rounded-lg hover:bg-indigo-700 transition-all duration-300 shadow-md hover:shadow-indigo-200"
            >
              Подать заявку
            </button>
          )}
        </div>

        {project.members.length === 0 ? (
          <div className="text-center py-10 bg-gray-50 rounded-xl">
            <p className="text-gray-500">Пока нет участников</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {project.members.map(member => (
              <div key={member.userId} className="flex items-center p-4 bg-white rounded-xl border border-gray-100 hover:shadow-md transition-shadow">
                <div className="relative">
                  {member.pictureUrl ? (
                    <img
                      src={`/user/images/${member.pictureUrl}`}
                      alt={member.userName}
                      className="w-14 h-14 rounded-full object-cover border-2 border-white shadow"
                    />
                  ) : (
                    <div className="w-14 h-14 rounded-full bg-gradient-to-r from-indigo-500 to-purple-500 text-white flex items-center justify-center font-bold text-xl shadow">
                      {member.userName.charAt(0).toUpperCase()}
                    </div>
                  )}
                  <span className="absolute -bottom-1 -right-1 bg-white px-2 py-0.5 rounded-full text-xs font-medium border border-gray-200 shadow-sm">
                    {roleTranslations[member.role] || member.role}
                  </span>
                </div>
                <div className="ml-4">
                  <Link
                    to={`/user/${member.userId}`}
                    className="text-lg font-medium text-gray-900 hover:text-indigo-600 transition-colors"
                  >
                    {member.userName}
                  </Link>
                </div>
              </div>
            ))}
          </div>
        )}
      </section>

      {/* Requests Section (for manager) */}
      {isManager && (
        <section className="mt-12">
          <h2 className="text-2xl font-bold text-gray-900 mb-6">Заявки на вступление</h2>

          {requestLoading ? (
            <div className="flex justify-center py-10">
              <Loader2 className="animate-spin w-8 h-8 text-indigo-600" />
            </div>
          ) : requests.length === 0 ? (
            <div className="text-center py-10 bg-gray-50 rounded-xl">
              <p className="text-gray-500">Нет новых заявок</p>
            </div>
          ) : (
            <div className="space-y-4">
              {requests.map(r => (
                <div key={r.id} className="p-5 bg-white rounded-xl border border-gray-100 shadow-sm hover:shadow-md transition-shadow">
                  <div className="flex justify-between items-start">
                    <div>
                      <Link
                        to={`/user/${r.userId}`}
                        className="text-lg font-medium text-indigo-600 hover:underline"
                      >
                        {r.userName}
                      </Link>
                      <p className="mt-1 text-gray-600">{r.message}</p>
                      <p className="mt-2 text-sm text-gray-500">{r.createdAt}</p>
                    </div>
                    <div className="flex space-x-2">
                      <button
                        onClick={() => {
                          setSelectedRequestId(r.id);
                          setShowRoleModal(true);
                        }}
                        className="px-4 py-2 bg-green-100 text-green-700 font-medium rounded-lg hover:bg-green-200 transition-colors"
                      >
                        Принять
                      </button>
                      <button
                        onClick={() => handleReject(r.id)}
                        className="px-4 py-2 bg-red-100 text-red-700 font-medium rounded-lg hover:bg-red-200 transition-colors"
                      >
                        Отклонить
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </section>
      )}

      {/* Join Request Modal */}
      {showModal && (
        <div className="fixed inset-0 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm z-50 animate-fade-in">
          <div className="w-full max-w-md bg-white rounded-2xl shadow-xl overflow-hidden transform transition-all duration-300 ease-out animate-scale-in">
            <div className="p-6">
              <h2 className="text-2xl font-bold text-gray-900 mb-2">Заявка на участие</h2>
              <p className="text-gray-600 mb-6">Расскажите, почему вы хотите присоединиться к проекту</p>

              <textarea
                value={joinMessage}
                onChange={(e) => setJoinMessage(e.target.value)}
                className="w-full h-32 p-4 border border-gray-200 rounded-xl focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                placeholder="Ваше сообщение..."
              />
            </div>

            <div className="px-6 py-4 bg-gray-50 flex justify-end space-x-3">
              <button
                onClick={() => setShowModal(false)}
                className="px-5 py-2.5 text-gray-700 font-medium rounded-lg hover:bg-gray-100 transition-colors"
              >
                Отмена
              </button>
              <button
                onClick={handleJoinRequest}
                className="px-5 py-2.5 bg-indigo-600 text-white font-medium rounded-lg hover:bg-indigo-700 transition-colors shadow-md hover:shadow-indigo-200"
              >
                Отправить
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Role Selection Modal */}
      {showRoleModal && (
        <div className="fixed inset-0 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm z-50 animate-fade-in">
          <div className="w-full max-w-md bg-white rounded-2xl shadow-xl overflow-hidden transform transition-all duration-300 ease-out animate-scale-in">
            <div className="p-6">
              <h2 className="text-2xl font-bold text-gray-900 mb-2">Назначить роль</h2>
              <p className="text-gray-600 mb-6">Выберите роль для нового участника</p>

              <select
                value={selectedRole}
                onChange={(e) => setSelectedRole(e.target.value)}
                className="w-full p-4 border border-gray-200 rounded-xl focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all appearance-none bg-[url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNCIgaGVpZ2h0PSIyNCIgdmlld0JveD0iMCAwIDI0IDI0IiBmaWxsPSJub25lIiBzdHJva2U9IiAjd2ViY2FmZSIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiIGNsYXNzPSJsdWNpZGUgbHVjaWRlLWNoZXZyb24tZG93biI+PHBhdGggZD0ibTYgOSA2IDYgNi02Ii8+PC9zdmc+')] bg-no-repeat bg-[center_right_1rem]"
              >
                <option value="">Выберите роль</option>
                <option value="Developer">Разработчик</option>
                <option value="Designer">Дизайнер</option>
                <option value="Tester">Тестировщик</option>
                <option value="ProjectManager">Менеджер проекта</option>
                <option value="BuisnessAnalyst">Бизнес-аналитик</option>
              </select>
            </div>

            <div className="px-6 py-4 bg-gray-50 flex justify-end space-x-3">
              <button
                onClick={() => setShowRoleModal(false)}
                className="px-5 py-2.5 text-gray-700 font-medium rounded-lg hover:bg-gray-100 transition-colors"
              >
                Отмена
              </button>
              <button
                onClick={handleConfirmRole}
                disabled={!selectedRole}
                className={`px-5 py-2.5 font-medium rounded-lg transition-colors shadow-md ${selectedRole
                    ? 'bg-indigo-600 text-white hover:bg-indigo-700 hover:shadow-indigo-200'
                    : 'bg-gray-300 text-gray-500 cursor-not-allowed'
                  }`}
              >
                Подтвердить
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}