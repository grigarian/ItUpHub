import axios from 'axios';
import { getCookie } from '../utils/cookies';

const api = axios.create({
  baseURL: process.env.NODE_ENV === 'development' 
    ? 'http://localhost:8081/api' 
    : '/api',
  withCredentials: true,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
api.interceptors.request.use(
  (config) => {
    const token = getCookie('tasty-cookies');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor
api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response?.status === 401) {
      // Удаляем куку при 401 ошибке
      document.cookie = 'tasty-cookies=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;';
    }
    return Promise.reject(error);
  }
);

export default api; 