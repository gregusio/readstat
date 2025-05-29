import apiClient from "./apiClient";

const getFollowing = async (userId: string) => {
    try {
        const response = await apiClient.get(`/Following/user/${userId}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching following users:", error);
        return [];
    }
}

const addFollowing = async (userId: string, followingId: string) => {
    try {
        const response = await apiClient.post(`/Following/user/${userId}/add/${followingId}`, {});
        return response.data;
    } catch (error) {
        console.error("Error adding following user:", error);
        throw error;
    }
}

const removeFollowing = async (userId: string, followingId: string) => {
    try {
        const response = await apiClient.delete(`/Following/user/${userId}/remove/${followingId}`);
        return response.data;
    } catch (error) {
        console.error("Error removing following user:", error);
        throw error;
    }
}

export default {
    getFollowing,
    addFollowing,
    removeFollowing
};