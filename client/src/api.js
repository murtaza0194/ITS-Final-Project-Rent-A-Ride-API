import axios from 'axios';
import { toast } from 'react-hot-toast';

const api = axios.create({
    baseURL: 'https://localhost:7068', // Adjust port if needed based on launchSettings
    headers: {
        'Content-Type': 'application/json',
    },
});

// Request Interceptor: Add Token
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Response Interceptor: Handle Errors
api.interceptors.response.use(
    (response) => response,
    (error) => {
        const message = error.response?.data?.message || 'Something went wrong';
        // Optimize: Don't show toast for 401 as we redirect
        if (error.response?.status !== 401) {
            toast.error(message);
        }

        if (error.response?.status === 401) {
            localStorage.removeItem('token');
            localStorage.removeItem('user'); // Clear user data
            window.location.href = '/login';
        }

        return Promise.reject(error);
    }
);

export const authAPI = {
    login: (email, password) => api.post('/Auth/login', { email, password }),
    register: (data) => api.post('/Auth/register', data),
};

export const vehicleAPI = {
    getAll: (page = 1, pageSize = 10) => api.get(`/Vehicles?pageNumber=${page}&pageSize=${pageSize}`),
    // Note: Backend currently doesn't support 'type' filter in controller, relying on client or future update
    getTypes: () => api.get('/Vehicles/types'),
    create: (data) => api.post('/Vehicles', data),
    delete: (id) => api.delete(`/Vehicles/${id}`),
};

export const rentalAPI = {
    book: (data) => api.post('/Rentals', data), // data: { userId, vehicleId, startDate, endDate, amenityIds }
    getMyHistory: (userId, page = 1) => api.get(`/Rentals/my-history?userId=${userId}&page=${page}`),
};

export default api;
