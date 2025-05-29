import apiClient from "./apiClient";

const getUsersStartingWith = async (searchTerm: string) => {
    try {
        const response = await apiClient.get(`/User/search/${searchTerm}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching users starting with:", error);
        return [];
    }
}

const getAllUsers = async (userId: string) => {
    try {
        const response = await apiClient.get(`/User/all/except/${userId}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching all users:", error);
        return [];
    }
}

const getUsernameById = async () => {
    try {
        const response = await apiClient.get(`/User/username`);
        return response.data.username;
    }
    catch (error) {
        console.error("Error fetching username by ID:", error);
        return null;
    }
}

export default {
    getUsersStartingWith,
    getAllUsers,
    getUsernameById
}