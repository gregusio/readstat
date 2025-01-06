import apiClient from './apiClient';

const login = async (credentials: { email: string; password: string }) => {
    const response = await apiClient.post('/Auth/login', credentials);
    const { accessToken, refreshToken } = response.data;

    if (accessToken) {
        localStorage.setItem('accessToken', accessToken); // Zapis tokena
    }

    if (refreshToken) {
        localStorage.setItem('refreshToken', refreshToken); // Zapis refreshTokena
    };

    return response.data;
};

const register = async (credentials: { email: string; password: string }) => {
    const response = await apiClient.post('/Auth/register', credentials);

    return response.data;
};


const logout = () => {
    localStorage.removeItem('accessToken'); 
    localStorage.removeItem('refreshToken');
};

const getCurrentUser = async () => {
    const response = await apiClient.get('/Auth/me'); // Endpoint zwracający dane użytkownika
    return response.data;
};

const refreshToken = async () => {
    const response = await apiClient.post('/Auth/refresh');
    const { accessToken } = response.data;

    if (accessToken) {
        localStorage.setItem('token', accessToken); // Zaktualizuj token
    }

    return accessToken;
};

export default {
    login,
    register,
    logout,
    getCurrentUser,
    refreshToken,
};
