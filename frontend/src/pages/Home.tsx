import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../api/axios';
import { ProjectWithCategory, Skill } from '../types';
import { Loader2, Users, Calendar, Target, Mail, MessageCircle } from 'lucide-react';
import { useAuth } from '../context/AuthContext';

export default function Home() {
  const { user } = useAuth();
  const [latestProjects, setLatestProjects] = useState<ProjectWithCategory[]>([]);
  const [popularSkills, setPopularSkills] = useState<Skill[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [projectsRes, skillsRes] = await Promise.all([
          api.get('/project/all-with-categories'),
          api.get('/skill/all')
        ]);
        
        // Берем только последние 3 проекта
        setLatestProjects(projectsRes.data.slice(0, 3));
        setPopularSkills(skillsRes.data.slice(0, 6));
      } catch (error) {
        console.error('Error fetching data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <Loader2 className="animate-spin w-10 h-10 text-indigo-600" />
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-white to-purple-50">
      {/* Hero Section */}
      <section className="relative overflow-hidden bg-gradient-to-r from-indigo-600 to-purple-600 text-white">
        <div className="absolute inset-0 bg-black/10"></div>
        <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-24">
          <div className="text-center">
            <h1 className="text-5xl md:text-6xl font-bold mb-6">
              Добро пожаловать в <span className="text-yellow-300">ItUpHub</span>
            </h1>
            <p className="text-xl md:text-2xl mb-8 text-indigo-100 max-w-3xl mx-auto">
              Платформа для поиска проектов, развития навыков и создания команды разработчиков
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <Link
                to="/projects"
                className="bg-white text-indigo-600 px-8 py-4 rounded-lg font-semibold text-lg hover:bg-gray-100 transition-colors shadow-lg"
              >
                Найти проект
              </Link>
              {user ? (
                <Link
                  to="/create-project"
                  className="border-2 border-white text-white px-8 py-4 rounded-lg font-semibold text-lg hover:bg-white hover:text-indigo-600 transition-colors"
                >
                  Создать проект
                </Link>
              ) : (
                <Link
                  to="/login"
                  className="border-2 border-white text-white px-8 py-4 rounded-lg font-semibold text-lg hover:bg-white hover:text-indigo-600 transition-colors"
                >
                  Войти в систему
                </Link>
              )}
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-4xl font-bold text-gray-900 mb-4">
              Почему выбирают ItUpHub?
            </h2>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto">
              Мы создаем экосистему для развития IT-сообщества
            </p>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            <div className="text-center p-6 rounded-xl bg-gradient-to-br from-purple-50 to-indigo-50 border border-purple-100">
              <div className="w-16 h-16 bg-purple-600 rounded-full flex items-center justify-center mx-auto mb-4">
                <Users className="w-8 h-8 text-white" />
              </div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">Найди команду</h3>
              <p className="text-gray-600">
                Находите единомышленников и создавайте сильные команды для реализации проектов
              </p>
            </div>
            
            <div className="text-center p-6 rounded-xl bg-gradient-to-br from-purple-50 to-indigo-50 border border-purple-100">
              <div className="w-16 h-16 bg-purple-600 rounded-full flex items-center justify-center mx-auto mb-4">
                <Target className="w-8 h-8 text-white" />
              </div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">Развивай навыки</h3>
              <p className="text-gray-600">
                Развивайте навыки через участие в реальных проектах и получайте опыт
              </p>
            </div>
            
            <div className="text-center p-6 rounded-xl bg-gradient-to-br from-purple-50 to-indigo-50 border border-purple-100">
              <div className="w-16 h-16 bg-purple-600 rounded-full flex items-center justify-center mx-auto mb-4">
                <Calendar className="w-8 h-8 text-white" />
              </div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">Запускай проекты</h3>
              <p className="text-gray-600">
                Создавайте и участвуйте в интересных проектах, которые меняют мир
              </p>
            </div>

            <div className="text-center p-6 rounded-xl bg-gradient-to-br from-purple-50 to-indigo-50 border border-purple-100">
              <div className="w-16 h-16 bg-purple-600 rounded-full flex items-center justify-center mx-auto mb-4">
                <Target className="w-8 h-8 text-white" />
              </div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">Достигай целей</h3>
              <p className="text-gray-600">
                Реализуйте свои идеи и достигайте профессиональных целей вместе с командой
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Latest Projects Section */}
      <section className="py-20 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center mb-12">
            <div>
              <h2 className="text-4xl font-bold text-gray-900 mb-2">
                Последние проекты
              </h2>
              <p className="text-xl text-gray-600">
                Присоединяйтесь к интересным проектам
              </p>
            </div>
            {user ? (
              <Link
                to="/projects"
                className="bg-indigo-600 text-white px-6 py-3 rounded-lg font-semibold hover:bg-indigo-700 transition-colors"
              >
                Все проекты
              </Link>
            ) : (
              <Link
                to="/login"
                className="bg-indigo-600 text-white px-6 py-3 rounded-lg font-semibold hover:bg-indigo-700 transition-colors"
              >
                Войти для просмотра
              </Link>
            )}
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {latestProjects.map((project) => (
              <div key={project.id} className="bg-white rounded-xl shadow-lg hover:shadow-xl transition-shadow p-6">
                <div className="flex items-center justify-between mb-4">
                  <span className="inline-block px-3 py-1 rounded-full text-sm font-medium bg-indigo-100 text-indigo-800">
                    {project.categoryName || 'Без категории'}
                  </span>
                  <span className={`px-2 py-1 rounded-full text-xs font-medium ${
                    project.status === 'InProgress' 
                      ? 'bg-yellow-100 text-yellow-800' 
                      : project.status === 'Complete' 
                      ? 'bg-green-100 text-green-800' 
                      : 'bg-red-100 text-red-800'
                  }`}>
                    {project.status === 'InProgress' ? 'В работе' : 
                     project.status === 'Complete' ? 'Завершен' : 'Отменен'}
                  </span>
                </div>
                <h3 className="text-xl font-semibold text-gray-900 mb-2">
                  {project.title}
                </h3>
                <p className="text-gray-600 mb-4 line-clamp-3">
                  {project.description}
                </p>
                <div className="flex items-center justify-between text-sm text-gray-500">
                  <span>Создан: {new Date(project.creationDate).toLocaleDateString('ru-RU')}</span>
                  {user ? (
                    <Link
                      to={`/project/${project.id}/get`}
                      className="text-indigo-600 hover:text-indigo-800 font-medium"
                    >
                      Подробнее →
                    </Link>
                  ) : (
                    <Link
                      to="/login"
                      className="text-indigo-600 hover:text-indigo-800 font-medium"
                    >
                      Войти для просмотра →
                    </Link>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Popular Skills Section */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-12">
            <h2 className="text-4xl font-bold text-gray-900 mb-4">
              Популярные навыки
            </h2>
            <p className="text-xl text-gray-600">
              Развивайте востребованные технологии
            </p>
          </div>
          
          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">
            {popularSkills.map((skill) => (
              <div key={skill.id.value} className="bg-gradient-to-br from-indigo-50 to-purple-50 rounded-xl p-4 text-center hover:shadow-md transition-shadow">
                <div className="w-12 h-12 bg-indigo-600 rounded-lg flex items-center justify-center mx-auto mb-3">
                  <span className="text-white font-bold text-lg">
                    {skill.title.value.charAt(0).toUpperCase()}
                  </span>
                </div>
                <h3 className="font-semibold text-gray-900">{skill.title.value}</h3>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Contact Section */}
      <section className="py-20 bg-gradient-to-r from-indigo-600 to-purple-600 text-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <h2 className="text-4xl font-bold mb-4">
            Свяжитесь с нами
          </h2>
          <p className="text-xl mb-8 text-indigo-100">
            Есть вопросы или предложения? Мы всегда на связи!
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <a
              href="mailto:grigarian24@mail.ru"
              className="flex items-center justify-center gap-2 bg-white text-indigo-600 px-6 py-3 rounded-lg font-semibold hover:bg-gray-100 transition-colors"
            >
              <Mail className="w-5 h-5" />
              grigarian24@mail.ru
            </a>
            <a
              href="https://t.me/grigarian24"
              target="_blank"
              rel="noopener noreferrer"
              className="flex items-center justify-center gap-2 bg-blue-500 text-white px-6 py-3 rounded-lg font-semibold hover:bg-blue-600 transition-colors"
            >
              <MessageCircle className="w-5 h-5" />
              @grigarian24
            </a>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-gray-900 text-white py-12">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div>
              <h3 className="text-2xl font-bold bg-gradient-to-r from-indigo-400 to-purple-400 bg-clip-text text-transparent mb-4">
                ItUpHub
              </h3>
              <p className="text-gray-400">
                Платформа для поиска проектов, развития навыков и создания команды разработчиков
              </p>
            </div>
            <div>
              <h4 className="text-lg font-semibold mb-4">Контакты</h4>
              <div className="space-y-2 text-gray-400">
                <p>Email: grigarian24@mail.ru</p>
                <p>Telegram: @grigarian24</p>
              </div>
            </div>
            <div>
              <h4 className="text-lg font-semibold mb-4">Правовая информация</h4>
              <div className="space-y-2">
                <Link to="/terms" className="block text-gray-400 hover:text-white transition-colors">
                  Условия использования
                </Link>
                <Link to="/privacy" className="block text-gray-400 hover:text-white transition-colors">
                  Политика конфиденциальности
                </Link>
              </div>
            </div>
          </div>
          <div className="border-t border-gray-800 mt-8 pt-8 text-center text-gray-400">
            <p>&copy; 2025 ItUpHub. Все права защищены.</p>
          </div>
        </div>
      </footer>
    </div>
  );
}