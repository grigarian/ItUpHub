// Базовые типы
export type Id = {
  value: string;
};

export type Title = {
  value: string;
};

// Типы проекта
export type Project = {
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

export type ProjectWithCategory = {
  id: string;
  title: string;
  description: string;
  categoryName?: string;
  status: string;
  startDate: string;
  endDate?: string;
  creationDate: string;
};

export type Member = {
  userId: string;
  userName: string;
  role: string;
  pictureUrl: string;
};

export type JoinRequestDto = {
  id: string;
  userId: string;
  userName: string;
  message: string;
  status: string;
  createdAt: string;
};

// Типы навыков
export type Skill = {
  id: Id;
  title: Title;
};

export type SkillDto = {
  id: string;
  title: string;
};

// Типы вакансий
export type ProjectVacancyDto = {
  id: string;
  projectId: string;
  title: string;
  description: string;
  createdAt: string;
  skills: SkillDto[];
  applications: VacancyApplicationDto[];
};

export type VacancyApplicationDto = {
  id: string;
  projectVacancyId: string;
  userId: string;
  userName: string;
  message: string;
  createdAt: string;
  status: string;
  managerComment?: string;
};

// Типы для запросов
export type CreateVacancyRequest = {
  title: string;
  description: string;
  skillIds: string[];
};

export type ApplyToVacancyRequest = {
  message: string;
};

export type ApproveApplicationRequest = {
  message: string;
};

export type RejectApplicationRequest = {
  message: string;
};

// Добавляем в конец файла
export type ApproveJoinRequestRequest = {
  role: string;
  projectId: string;
};

// Константы перевода
export const roleTranslations: Record<string, string> = {
  ProjectManager: 'Руководитель проекта',
  Developer: 'Разработчик',
  Designer: 'Дизайнер',
  BuisnessAnalyst: 'Бизнес-аналитик',
  Tester: 'Тестировщик',
};

export const statusTranslations: Record<string, string> = {
  InProgress: 'Активен',
  Complete: 'Завершён',
  Cancelled: 'Отменён',
  Pending: 'На рассмотрении',
  Approved: 'Принята',
  Rejected: 'Отклонена',
};

export type Category = {
  id: string;
  title: string;
};