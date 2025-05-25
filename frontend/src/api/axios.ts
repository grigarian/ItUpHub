import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:8080',
  withCredentials: true, // если используешь куки
});

api.defaults.withCredentials = true;

export default api;