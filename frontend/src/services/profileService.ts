import apiClient from "./apiClient";

const getProfile = async () => {
  try {
    const response = await apiClient.get("/Profile/user");

    return response.data;
  } catch (error) {
    console.error("Error fetching profile data:", error);
    return null;
  }
};

const updateProfile = async (profileData: any) => {
  try {
    const response = await apiClient.put("/Profile/user", profileData);

    return response.data;
  } catch (error) {
    console.error("Error updating profile data:", error);
    return null;
  }
}

export default {
  getProfile,
  updateProfile,
};