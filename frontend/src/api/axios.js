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
    console.log('Request to:', config.url);
    console.log('Request method:', config.method);
    console.log('With credentials:', config.withCredentials);
    
    const token = getCookie('tasty-cookies');
    console.log('Token in request interceptor:', token ? 'found' : 'not found');
    
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
      console.log('Authorization header set');
    }
    return config;
  },
  (error) => {
    console.error('Request interceptor error:', error);
    return Promise.reject(error);
  }
);

// Response interceptor
api.interceptors.response.use(
  (response) => {
    console.log('Response from:', response.config.url);
    console.log('Response status:', response.status);
    console.log('Response headers:', response.headers);
    return response;
  },
  (error) => {
    console.error('Response error:', error);
    console.error('Response error status:', error.response?.status);
    console.error('Response error data:', error.response?.data);
    
    if (error.response?.status === 401) {
      // Удаляем куку при 401 ошибке
      document.cookie = 'tasty-cookies=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;';
      console.log('401 error - cookie deleted');
    }
    return Promise.reject(error);
  }
);

export default api; 