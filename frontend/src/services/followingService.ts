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

const addFollowing = async (followingId: string) => {
    try {
        const response = await apiClient.post(`/Following/add/${followingId}`, {});
        return response.data;
    } catch (error) {
        console.error("Error adding following user:", error);
        throw error;
    }
}

const removeFollowing = async (followingId: string) => {
    try {
        const response = await apiClient.delete(`/Following/remove/${followingId}`);
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