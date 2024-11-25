import axios from 'axios';
import { useLoading } from '@/composables/useLoading';

const { startLoading, stopLoading } = useLoading();

const api = axios.create({
  baseURL: 'https://localhost:7216/api/'
});

api.interceptors.request.use(
  (config) => {
    startLoading(); 
    return config;
  },
  (error) => {
    stopLoading(); 
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => {
    stopLoading();
    return response;
  },
  (error) => {
    stopLoading();
    return Promise.reject(error);
  }
);

export default api;
