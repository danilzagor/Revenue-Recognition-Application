import axios from 'axios';

const axiosInstance = axios.create({
    baseURL: 'https://localhost:7050',
    withCredentials: true
});

axiosInstance.interceptors.request.use(
    (config) => {
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

axiosInstance.interceptors.response.use(
    (response) => {
        return response;
    },
    async (error) => {
        const originalRequest = error.config;
        if (error.response && error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            try {
                const response = await axios.post(
                    'https://localhost:7050/auth/refresh',
                    {},
                    { withCredentials: true }
                );


                return axiosInstance(originalRequest);
            } catch (e) {
                console.error('Refresh token is invalid:', e);
            }
        }
        return Promise.reject(error);
    }
);

export default axiosInstance;
