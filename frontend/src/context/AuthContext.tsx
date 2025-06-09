import { createContext, useContext, useState, useEffect } from "react";
import api from "../api/axios";   
import { useNavigate } from "react-router-dom";
import axios from "axios";

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
  isAdmin: boolean;
};

type AuthContextType = {
  user: User | null;
  login: (user: User) => void;
  logout: () => Promise<void>;
  updateUser: (updates: Partial<User>) => void;
  refreshUser: () => Promise<void>;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isInitialized, setIsInitialized] = useState(false);
  const navigate = useNavigate();

  const loadUser = async () => {
    if (!isInitialized) {
      try {
        const { data } = await api.get('/user/current', { 
          withCredentials: true 
        });
        setUser(mapUserResponseToUser(data));
      } catch (error: any) {
        if (error.response?.status === 401) {
          setUser(null);
        } else {
          console.error('Ошибка при загрузке пользователя:', error);
          setUser(null);
        }
      } finally {
        setIsInitialized(true);
      }
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
    isAdmin: data.isAdmin || false,
  });

  async function refreshUser() {
    try {
      const { data } = await api.get('/user/current', { withCredentials: true });
      setUser(mapUserResponseToUser(data));
    } catch (error: unknown) {
      console.error('Ошибка при обновлении пользователя:', error);
      if (axios.isAxiosError(error) && error.response?.status === 401) {
        setUser(null);
      }
    }
  }

  const logout = async () => {
    try {
      await api.post("/user/logout", {}, { 
        withCredentials: true 
      });
    } catch (error) {
      console.error("Logout error:", error);
    } finally {
      setUser(null);
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

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider");
  }
  return context;
};