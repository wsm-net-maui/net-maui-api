import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7194/api', // Ajuste conforme a porta real da sua API .NET
  timeout: 10000,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
