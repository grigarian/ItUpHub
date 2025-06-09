import { useEffect, useState } from 'react';
import api from '../api/axios';
import { useAuth } from '../context/AuthContext';
import { Link } from 'react-router-dom';
import { toast } from 'react-hot-toast';
import { Loader2 } from 'lucide-react';

type Project = {
  id: string;
  title: string;
  description: string;
  category: string;
};

export default function MyProjectsPage() {
  const { user } = useAuth();
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!user) return;

    api.get(`/project/all-titles/${user.id}`, { withCredentials: true })
      .then(res => setProjects(res.data))
      .catch(() => toast.error('Ошибка загрузки проектов'))
      .finally(() => setLoading(false));
  }, [user]);

  return (
    <div className="max-w-4xl mx-auto mt-8 p-6 bg-white/90 backdrop-blur-lg rounded-3xl shadow-[0_8px_30px_rgb(0,0,0,0.12)] border border-gray-100">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-8 gap-4">
        <h2 className="text-3xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
          Мои проекты
        </h2>
        <Link
          to="/create-project"
          className="px-6 py-2.5 bg-indigo-600 hover:bg-indigo-700 text-white font-medium rounded-lg transition-colors shadow-md hover:shadow-indigo-200 flex items-center gap-2"
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
            <path fillRule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clipRule="evenodd" />
          </svg>
          Создать проект
        </Link>
      </div>

      {loading ? (
        <div className="flex justify-center py-10">
          <Loader2 className="animate-spin w-10 h-10 text-indigo-600" />
        </div>
      ) : projects.length === 0 ? (
        <div className="text-center p-8 bg-gray-50 rounded-xl">
          <p className="text-gray-600 mb-4">У вас пока нет проектов.</p>
          <Link 
            to="/create-project" 
            className="inline-flex items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white font-medium rounded-lg transition-colors shadow hover:shadow-indigo-200"
          >
            Создать первый проект
            <svg xmlns="http://www.w3.org/2000/svg" className="ml-2 h-4 w-4" viewBox="0 0 20 20" fill="currentColor">
              <path fillRule="evenodd" d="M10.293 5.293a1 1 0 011.414 0l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414-1.414L12.586 11H5a1 1 0 110-2h7.586l-2.293-2.293a1 1 0 010-1.414z" clipRule="evenodd" />
            </svg>
          </Link>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
          {projects.map((project) => (
            <Link
              key={project.id}
              to={`/project/${project.id}/get`}
              className="group border border-gray-200 p-5 rounded-xl hover:shadow-md transition-all duration-300 hover:border-indigo-100 hover:-translate-y-1"
            >
              <div className="flex flex-col h-full">
                <h3 className="text-xl font-semibold text-gray-900 group-hover:text-indigo-600 transition-colors mb-2">
                  {project.title}
                </h3>
                <p className="text-gray-600 mb-4 flex-grow truncate">{project.description}</p>
                <div className="mt-auto">
                  <span className="inline-block text-sm font-medium px-3 py-1 rounded-full bg-indigo-100 text-indigo-800">
                    {project.category}
                  </span>
                </div>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}