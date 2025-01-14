import apiClient from "./apiClient";

const getSummary = async () => {
  try {
    const response = await apiClient.get("/Statistic/summary");
    return response.data;
  } catch (error) {
    console.error("Error fetching summary:", error);
    return null;
  }
};

const getBooksRead = async () => {
  try {
    const response = await apiClient.get("/Statistic/books-read");
    return response.data;
  } catch (error) {
    console.error("Error fetching books read:", error);
    return null;
  }
};

const getProgress = async () => {
  try {
    const response = await apiClient.get("/Statistic/progress");
    return response.data;
  } catch (error) {
    console.error("Error fetching progress:", error);
    return null;
  }
};

const getMonthlyReadBookCountPerYear = async () => {
  try {
    const response = await apiClient.get("/Statistics/monthly-read-books");

    return response.data;
  } catch (error) {
    console.error("Error fetching monthly read book count per year:", error);
    return null;
  }
};

const getMonthlyReadPageCountPerYear = async () => {
  try {
    const response = await apiClient.get("/Statistics/monthly-read-pages");

    return response.data;
  } catch (error) {
    console.error("Error fetching monthly read page count per year:", error);
    return null;
  }
};

const getMonthlyAddedBookCountPerYear = async () => {
  try {
    const response = await apiClient.get("/Statistics/monthly-added-books");

    return response.data;
  } catch (error) {
    console.error("Error fetching monthly added book count per year:", error);
    return null;
  }
};

const getYearlyReadBookCount = async () => {
  try {
    const response = await apiClient.get("/Statistics/yearly-read-books");

    return response.data;
  } catch (error) {
    console.error("Error fetching yearly read book count:", error);
    return null;
  }
};

export default {
  getSummary,
  getBooksRead,
  getProgress,
  getMonthlyReadBookCountPerYear,
  getMonthlyReadPageCountPerYear,
  getMonthlyAddedBookCountPerYear,
  getYearlyReadBookCount,
};
