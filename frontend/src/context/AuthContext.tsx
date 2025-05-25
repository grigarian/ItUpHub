import { createContext, useContext, useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

type Skill = {
  id: string;
  title: string;
}

type User = {
  id: string;
  userName: string;
  email: string;
  avatar: string;
  bio: string;
  skills: Skill[];
};

type AuthContextType = {
  user: User | null;
  login: (user: User) => void;
  logout: () => Promise<void>;
  updateUser: (updates: Partial<User>) => void;
  refreshUser: () => Promise<void>;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);
axios.defaults.withCredentials = true;

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isInitialized, setIsInitialized] = useState(false);
  const navigate = useNavigate();
  /*const api = axios.create({
  baseURL: process.env.NODE_ENV === 'development' 
    ? 'http://localhost:3000/api' 
    : '/api',
  withCredentials: true,
});*/
console.log('Текущий пользователь:', user);
  const loadUser = async () => {
    try {
      const { data } = await axios.get('/user/current', { 
        withCredentials: true 
      });
      setUser(mapUserResponseToUser(data));
    } catch (error) {
      setUser(null);
    } finally {
      setIsInitialized(true);
    }
  };

  const updateUser = (updates: Partial<User>) => {
    if (user) {
      setUser({ ...user, ...updates });
    }
  };

  const mapUserResponseToUser = (data: any): User => ({
    id: data.id,
    userName: data.userName,
    email: data.email,
    avatar: data.avatar
      ? `${data.avatar}?t=${Date.now()}`
      : '/default-avatar.png',
    bio: data.bio || '',
    skills: data.skills || [],
  });

  async function refreshUser() {
    try {
      const { data } = await axios.get('/user/current', { withCredentials: true });
      setUser(mapUserResponseToUser(data));
    } catch (error) {
      console.error('Ошибка при обновлении пользователя:', error);
    }
  }

  const logout = async () => {
    try {
      // 1. Отправляем запрос на бэкенд для выхода
      await axios.post("/user/logout", {}, { 
        withCredentials: true 
      });
    } catch (error) {
      console.error("Logout error:", error);
    } finally {
      // 2. Очищаем состояние в любом случае
      setUser(null);
      // 3. Перенаправляем на главную
      navigate("/");
    }
  };

  useEffect(() => {
    loadUser();
  }, []);

  const login = (userData: User) => {
    setUser(userData);
  };

  return (
    <AuthContext.Provider value={{ user, updateUser, login, logout, refreshUser }}>
      {children}
    </AuthContext.Provider>
  );
};

// Убедись, что хук экспортируется как именованный экспорт
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider");
  }
  return context;
};