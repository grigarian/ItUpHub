import React, { useState } from 'react';
import api from '../api/axios';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';
import { toast } from 'react-hot-toast';
import { useAuth } from '../context/AuthContext';
import { getErrorMessage } from '../utils/errorMessages';
import { Loader2, Eye, EyeOff } from 'lucide-react';
import { useSEO } from '../utils/hooks/useSEO';
import { SEO_CONFIGS } from '../utils/seo';

type LoginData = {
  email: string;
  password: string;
};

export default function Login() {
  const [form, setForm] = useState<LoginData>({ email: '', password: '' });
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  // SEO оптимизация для страницы входа
  useSEO(SEO_CONFIGS.login);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const { data: userData } = await api.post('/user/login', form, { withCredentials: true });
      
      login({
        id: userData.id,
        userName: userData.userName,
        email: userData.email,
        avatar: userData.avatar ? `${userData.avatar}` : '',
        bio: userData.bio || '',
        skills: userData.skills || [],
        isAdmin: false
      });
      
      toast.success('Вы успешно вошли!');
      navigate('/');
    } catch (error: unknown) {
      console.error('Login error:', error);
      if (axios.isAxiosError(error) && error.response?.data) {
        const message = getErrorMessage(error.response.data);
        toast.error(message);
      } else {
        toast.error('Что-то пошло не так. Попробуйте снова.');
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-50 to-purple-50 p-4">
      <div className="w-full max-w-md bg-white/90 backdrop-blur-lg rounded-2xl shadow-xl overflow-hidden border border-gray-100">
        <div className="p-8">
          <div className="text-center mb-8">
            <h2 className="text-3xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
              Добро пожаловать
            </h2>
            <p className="text-gray-500 mt-2">Введите свои данные для входа</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="space-y-2">
              <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                Email
              </label>
              <div className="relative">
                <input
                  id="email"
                  name="email"
                  type="email"
                  value={form.email}
                  onChange={handleChange}
                  required
                  className="w-full px-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                  placeholder="your@email.com"
                />
              </div>
            </div>

            <div className="space-y-2">
              <label htmlFor="password" className="block text-sm font-medium text-gray-700">
                Пароль
              </label>
              <div className="relative">
                <input
                  id="password"
                  name="password"
                  type={showPassword ? 'text' : 'password'}
                  value={form.password}
                  onChange={handleChange}
                  required
                  className="w-full px-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all pr-10"
                  placeholder="••••••••"
                />
                <button
                  type="button"
                  className="absolute right-3 top-3.5 text-gray-400 hover:text-gray-600"
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                </button>
              </div>
            </div>

            <div className="flex items-center justify-between">
              <div className="flex items-center">
                <input
                  id="remember-me"
                  name="remember-me"
                  type="checkbox"
                  className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                />
                <label htmlFor="remember-me" className="ml-2 block text-sm text-gray-700">
                  Запомнить меня
                </label>
              </div>

              <Link 
                to="/forgot-password" 
                className="text-sm text-indigo-600 hover:text-indigo-500 hover:underline"
              >
                Забыли пароль?
              </Link>
            </div>

            <button
              type="submit"
              disabled={isLoading}
              className="w-full flex justify-center items-center py-3 px-4 bg-gradient-to-r from-indigo-600 to-purple-600 hover:from-indigo-700 hover:to-purple-700 text-white font-medium rounded-lg shadow-md hover:shadow-indigo-200 transition-all duration-300 disabled:opacity-70"
            >
              {isLoading ? (
                <>
                  <Loader2 className="animate-spin mr-2 h-4 w-4" />
                  Вход...
                </>
              ) : (
                'Войти в аккаунт'
              )}
            </button>
          </form>

          <div className="mt-6 text-center">
            <p className="text-sm text-gray-600">
              Ещё нет аккаунта?{' '}
              <Link 
                to="/register" 
                className="font-medium text-indigo-600 hover:text-indigo-500 hover:underline"
              >
                Зарегистрироваться
              </Link>
            </p>
          </div>
        </div>

        <div className="px-8 py-4 bg-gray-50 text-center border-t border-gray-100">
          <p className="text-xs text-gray-500">
            Нажимая «Войти», вы соглашаетесь с нашими Условиями и Политикой конфиденциальности
          </p>
        </div>
      </div>
    </div>
  );
}