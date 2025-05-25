import { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-hot-toast';
import { Loader2, Calendar, X } from 'lucide-react';

type Category = {
  id: string;
  title: string;
};

export default function CreateProjectPage() {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [categoryId, setCategoryId] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isCategoriesLoading, setIsCategoriesLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    setIsCategoriesLoading(true);
    axios.get('category/all')
      .then(res => setCategories(res.data))
      .catch(() => toast.error('Ошибка загрузки категорий'))
      .finally(() => setIsCategoriesLoading(false));
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      await axios.post('/project/create', {
        title,
        description,
        categoryId,
        startDate,
        endDate: endDate || null,
      }, { withCredentials: true });

      toast.success('Проект успешно создан!');
      navigate('/my-projects');
    } catch (error) {
      toast.error( 'Ошибка при создании проекта');
    } finally {
      setIsLoading(false);
    }
  };

  const clearEndDate = () => {
    setEndDate('');
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-50 to-purple-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-2xl mx-auto bg-white/90 backdrop-blur-lg rounded-2xl shadow-xl overflow-hidden border border-gray-100">
        <div className="p-8 sm:p-10">
          <div className="text-center mb-8">
            <h2 className="text-3xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
              Создать новый проект
            </h2>
            <p className="mt-2 text-gray-600">
              Заполните информацию о вашем проекте
            </p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label htmlFor="title" className="block text-sm font-medium text-gray-700 mb-1">
                Название проекта
              </label>
              <input
                id="title"
                type="text"
                placeholder="Введите название проекта"
                value={title}
                onChange={e => setTitle(e.target.value)}
                required
                className="w-full px-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
              />
            </div>

            <div>
              <label htmlFor="description" className="block text-sm font-medium text-gray-700 mb-1">
                Описание проекта
              </label>
              <textarea
                id="description"
                placeholder="Опишите ваш проект..."
                value={description}
                onChange={e => setDescription(e.target.value)}
                required
                rows={4}
                className="w-full px-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
              />
            </div>

            <div>
              <label htmlFor="category" className="block text-sm font-medium text-gray-700 mb-1">
                Категория
              </label>
              {isCategoriesLoading ? (
                <div className="flex items-center justify-center py-3">
                  <Loader2 className="animate-spin h-5 w-5 text-indigo-600" />
                </div>
              ) : (
                <select
                  id="category"
                  value={categoryId}
                  onChange={e => setCategoryId(e.target.value)}
                  required
                  className="w-full px-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all appearance-none bg-[url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNCIgaGVpZ2h0PSIyNCIgdmlld0JveD0iMCAwIDI0IDI0IiBmaWxsPSJub25lIiBzdHJva2U9IiAjd2ViY2FmZSIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiIGNsYXNzPSJsdWNpZGUgbHVjaWRlLWNoZXZyb24tZG93biI+PHBhdGggZD0ibTYgOSA2IDYgNi02Ii8+PC9zdmc+')] bg-no-repeat bg-[center_right_1rem]"
                >
                  <option value="">Выберите категорию</option>
                  {categories.map(cat => (
                    <option key={cat.id} value={cat.id}>
                      {cat.title}
                    </option>
                  ))}
                </select>
              )}
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label htmlFor="startDate" className="block text-sm font-medium text-gray-700 mb-1">
                  Дата начала
                </label>
                <div className="relative">
                  <input
                    id="startDate"
                    type="date"
                    value={startDate}
                    onChange={e => setStartDate(e.target.value)}
                    required
                    className="w-full px-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all pl-10"
                  />
                  <Calendar className="absolute left-3 top-3.5 h-5 w-5 text-gray-400" />
                </div>
              </div>

              <div>
                <label htmlFor="endDate" className="block text-sm font-medium text-gray-700 mb-1">
                  Дата окончания (необязательно)
                </label>
                <div className="relative">
                  <input
                    id="endDate"
                    type="date"
                    value={endDate}
                    onChange={e => setEndDate(e.target.value)}
                    min={startDate}
                    className="w-full px-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all pl-10"
                  />
                  <Calendar className="absolute left-3 top-3.5 h-5 w-5 text-gray-400" />
                  {endDate && (
                    <button
                      type="button"
                      onClick={clearEndDate}
                      className="absolute right-3 top-3.5 text-gray-400 hover:text-gray-600"
                    >
                      <X className="h-5 w-5" />
                    </button>
                  )}
                </div>
              </div>
            </div>

            <div className="pt-4">
              <button
                type="submit"
                disabled={isLoading || isCategoriesLoading}
                className="w-full flex justify-center items-center py-3 px-4 bg-gradient-to-r from-indigo-600 to-purple-600 hover:from-indigo-700 hover:to-purple-700 text-white font-medium rounded-lg shadow-md hover:shadow-indigo-200 transition-all duration-300 disabled:opacity-70"
              >
                {isLoading ? (
                  <>
                    <Loader2 className="animate-spin mr-2 h-4 w-4" />
                    Создание...
                  </>
                ) : (
                  'Создать проект'
                )}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}