import { useState } from 'react';
import { ProjectVacancyDto, VacancyApplicationDto } from '../types';
import { Link } from 'react-router-dom';

type VacancyApplicationsModalProps = {
    isOpen: boolean;
    onClose: () => void;
    vacancy: ProjectVacancyDto | null;
    isManager: boolean;
    currentUserId: string;
    onApply: (vacancyId: string, message: string) => void;
    onApprove: (applicationId: string, message: string, role: string) => void;
    onReject: (applicationId: string, message: string) => void;
};

const ROLES = [
    'Developer',
    'Designer',
    'Tester',
    'ProjectManager',
    'BuisnessAnalyst'
];

export default function VacancyApplicationsModal({
    isOpen,
    onClose,
    vacancy,
    isManager,
    currentUserId,
    onApply,
    onApprove,
    onReject
}: VacancyApplicationsModalProps) {
    const [applicationMessage, setApplicationMessage] = useState('');
    const [approveMessage, setApproveMessage] = useState('');
    const [rejectMessage, setRejectMessage] = useState('');
    const [currentApplicationId, setCurrentApplicationId] = useState<string | null>(null);
    const [selectedRole, setSelectedRole] = useState('');

    if (!isOpen || !vacancy) return null;

    const userApplication = vacancy.applications.find(app => app.userId === currentUserId);
    const isUserApplied = !!userApplication;

    const handleApply = () => {
        if (applicationMessage.trim()) {
            onApply(vacancy.id, applicationMessage);
            setApplicationMessage('');
        }
    };

    const handleApprove = (applicationId: string) => {
        if (approveMessage.trim() && selectedRole) {
            onApprove(applicationId, approveMessage, selectedRole);
            setApproveMessage('');
            setSelectedRole(''); // Сбрасываем роль
            setCurrentApplicationId(null);
        }
    };

    const handleReject = (applicationId: string) => {
        if (rejectMessage.trim()) {
            onReject(applicationId, rejectMessage);
            setRejectMessage('');
            setCurrentApplicationId(null);
        }
    };
    console.log(vacancy)

    return (
        <div className="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
            <div className="bg-white rounded-xl p-6 w-full max-w-2xl shadow-lg max-h-[90vh] overflow-y-auto">
                <div className="flex justify-between items-start mb-4">
                    <h2 className="text-xl font-bold">Вакансия: {vacancy.title}</h2>
                    <button onClick={onClose} className="text-gray-500 hover:text-gray-700">
                        ✕
                    </button>
                </div>

                {!isManager && !isUserApplied && (
                    <div className="mb-6">
                        <h3 className="text-lg font-medium mb-2">Подать заявку</h3>
                        <textarea
                            value={applicationMessage}
                            onChange={(e) => setApplicationMessage(e.target.value)}
                            className="w-full border rounded-lg p-3 h-24 mb-3"
                            placeholder="Расскажите о своем опыте и почему вы подходите..."
                        />
                        <button
                            onClick={handleApply}
                            disabled={!applicationMessage.trim()}
                            className={`px-4 py-2 rounded-lg ${!applicationMessage.trim()
                                ? 'bg-gray-300 text-gray-500 cursor-not-allowed'
                                : 'bg-green-600 text-white hover:bg-green-700'
                                }`}
                        >
                            Отправить заявку
                        </button>
                    </div>
                )}

                {isManager && vacancy.applications.length > 0 && (
                    <div>
                        <h3 className="text-lg font-medium mb-3">Заявки на вакансию</h3>
                        <div className="space-y-4">
                            {vacancy.applications.map(application => (
                                <div key={application.id} className="p-4 border border-gray-200 rounded-lg">
                                    <div className="flex justify-between">
                                        <div>
                                            <Link
                                                to={`/user/${application.userId}`}
                                                className="text-lg font-medium text-gray-900 hover:text-indigo-600"
                                            >
                                                {application.userName}
                                            </Link>
                                            <p className="text-sm text-gray-600 mt-1">{application.message}</p>
                                            <p className="text-xs text-gray-500 mt-2">
                                                {new Date(application.createdAt).toLocaleString()}
                                            </p>
                                        </div>
                                        <div className="flex flex-col space-y-2">
                                            <button
                                                onClick={() => {
                                                    setApproveMessage('');
                                                    setRejectMessage('');
                                                    setCurrentApplicationId(application.id);
                                                }}
                                                className="text-sm text-indigo-600 hover:text-indigo-800"
                                            >
                                                Действия
                                            </button>
                                            {application.managerComment && (
                                                <p className="text-xs text-gray-500">
                                                    Комментарий: {application.managerComment}
                                                </p>
                                            )}
                                        </div>
                                    </div>

                                    {currentApplicationId === application.id && (
                                        <div className="mt-3 pt-3 border-t border-gray-200">
                                            <select
                                                value={selectedRole}
                                                onChange={(e) => setSelectedRole(e.target.value)}
                                                className="w-full border rounded-lg p-2 mb-2 text-sm"
                                                required
                                            >
                                                <option value="">Выберите роль</option>
                                                {ROLES.map(role => (
                                                    <option key={role} value={role}>{role}</option>
                                                ))}
                                            </select>
                                            <textarea
                                                value={approveMessage}
                                                onChange={(e) => setApproveMessage(e.target.value)}
                                                className="w-full border rounded-lg p-2 mb-2 text-sm"
                                                placeholder="Сообщение для пользователя (при принятии)"
                                                rows={2}
                                            />
                                            <div className="flex space-x-2">
                                                <button
                                                    onClick={() => handleApprove(application.id)}
                                                    disabled={!approveMessage.trim() || !selectedRole} // Блокируем если нет роли
                                                    className={`px-3 py-1 rounded text-sm ${!approveMessage.trim() || !selectedRole
                                                        ? 'bg-gray-300 text-gray-500 cursor-not-allowed'
                                                        : 'bg-green-100 text-green-700'
                                                        }`}
                                                >
                                                    Принять
                                                </button>
                                                <button
                                                    onClick={() => handleReject(application.id)}
                                                    className="px-3 py-1 bg-red-100 text-red-700 rounded text-sm"
                                                >
                                                    Отклонить
                                                </button>
                                            </div>
                                        </div>
                                    )}
                                </div>
                            ))}
                        </div>
                    </div>
                )}

                {!isManager && isUserApplied && (
                    <div className="p-4 bg-blue-50 rounded-lg">
                        <p className="text-blue-700">
                            Вы уже подали заявку на эту вакансию. Статус: {userApplication.status}
                        </p>
                        {userApplication.managerComment && (
                            <p className="mt-2 text-sm text-blue-600">
                                Комментарий менеджера: {userApplication.managerComment}
                            </p>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
}