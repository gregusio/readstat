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


export default {
  getFeed,
};