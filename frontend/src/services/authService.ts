import apiClient from "./apiClient";

const login = async (credentials: { username: string; password: string }) => {
  const response = await apiClient.post("/Auth/login", credentials);
  const { accessToken, refreshToken, success } = response.data;

  if (!success) {
    throw new Error("Invalid login response");
  }

  if (accessToken) {
    localStorage.setItem("accessToken", accessToken); 
  }

  if (refreshToken) {
    localStorage.setItem("refreshToken", refreshToken); 
  }

  return response.data;
};

const register = async (credentials: { username: string; password: string }) => {
  const response = await apiClient.post("/Auth/register", credentials);

  return response.data;
};

const logout = () => {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
};

const getCurrentUser = async () => {
  const response = await apiClient.get("/Auth/me"); 
  return response.data;
};

const refreshToken = async () => {
  const response = await apiClient.post("/Auth/refresh");
  const { accessToken } = response.data;

  if (accessToken) {
    localStorage.setItem("token", accessToken); 
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
