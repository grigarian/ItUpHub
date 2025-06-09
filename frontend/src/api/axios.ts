import axios from 'axios';

const api = axios.create({
  baseURL: process.env.NODE_ENV === 'development' 
    ? 'http://localhost:8081/api' 
    : '/api',
  withCredentials: true,
});

// Add response interceptor for error handling


export default api;