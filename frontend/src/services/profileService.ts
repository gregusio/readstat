import apiClient from "./apiClient";

const getProfile = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Profile/user/${userId}`);

    return response.data;
  } catch (error) {
    console.error("Error fetching profile data:", error);
    return null;
  }
};

const updateProfile = async (profileData: any, userId: string) => {
  try {
    const response = await apiClient.put(`/Profile/user/${userId}`, profileData);

    return response.data;
  } catch (error) {
    console.error("Error updating profile data:", error);
    return null;
  }
}

const getUserActivityHistory = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Profile/user/${userId}/activity-history`);
    return response.data;
  } catch (error) {
    console.error("Error fetching user activity history:", error);
    return null;
  }
};

export default {
  getProfile,
  updateProfile,
  getUserActivityHistory,
};