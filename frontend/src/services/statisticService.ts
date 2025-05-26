import apiClient from "./apiClient";

const getSummary = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/summary`);
    return response.data;
  } catch (error) {
    console.error("Error fetching summary:", error);
    return null;
  }
};

const getBooksRead = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/books-read`);
    return response.data;
  } catch (error) {
    console.error("Error fetching books read:", error);
    return null;
  }
};

const getProgress = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/progress`);
    return response.data;
  } catch (error) {
    console.error("Error fetching progress:", error);
    return null;
  }
};

const getMonthlyReadBookCountPerYear = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/monthly-read-books`);

    return response.data;
  } catch (error) {
    console.error("Error fetching monthly read book count per year:", error);
    return null;
  }
};

const getMonthlyReadPageCountPerYear = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/monthly-read-pages`);

    return response.data;
  } catch (error) {
    console.error("Error fetching monthly read page count per year:", error);
    return null;
  }
};

const getMonthlyAddedBookCountPerYear = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/monthly-added-books`);

    return response.data;
  } catch (error) {
    console.error("Error fetching monthly added book count per year:", error);
    return null;
  }
};

const getYearlyReadBookCount = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/yearly-read-books`);

    return response.data;
  } catch (error) {
    console.error("Error fetching yearly read book count:", error);
    return null;
  }
};

const getYearlyReadPageCount = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/yearly-read-pages`);

    return response.data;
  } catch (error) {
    console.error("Error fetching yearly read page count:", error);
    return null;
  }
};

const getYearlyAddedBookCount = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/yearly-added-books`);

    return response.data;
  } catch (error) {
    console.error("Error fetching yearly added book count:", error);
    return null;
  }
};

const getMostReadAuthors = async (userId: string) => {
  try {
    const response = await apiClient.get(`/Statistic/user/${userId}/most-read-authors`);

    return response.data;
  } catch (error) {
    console.error("Error fetching most read authors:", error);
    return null;
  }
}

export default {
  getSummary,
  getBooksRead,
  getProgress,
  getMonthlyReadBookCountPerYear,
  getMonthlyReadPageCountPerYear,
  getMonthlyAddedBookCountPerYear,
  getYearlyReadBookCount,
  getYearlyReadPageCount,
  getYearlyAddedBookCount,
  getMostReadAuthors,
};
