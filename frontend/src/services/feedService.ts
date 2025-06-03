import apiClient from "./apiClient";

const getFeed = async () => {
  try {
    const response = await apiClient.get(`/Feed/get`);
    return response.data;
  } catch (error) {
    console.error("Error fetching feed data:", error);
    return null;
  }
};  

const likeActivity = async (activityId: number) => {
  try {
    await apiClient.post(`/Feed/like/${activityId}`);
  } catch (error) {
    console.error("Error liking activity:", error);
  }
};

const unlikeActivity = async (activityId: number) => {
  try {
    await apiClient.post(`/Feed/unlike/${activityId}`);
  } catch (error) {
    console.error("Error unliking activity:", error);
  }
};

export default {
  getFeed,
  likeActivity,
  unlikeActivity,
};