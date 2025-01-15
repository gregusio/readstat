import apiClient from "./apiClient";

const getUserBooks = async () => {
  const userBooks = localStorage.getItem("userBooks");
  const response = await apiClient.get(`/Book/user-books`);
  
  if (userBooks && userBooks !== "undefined") {
    return JSON.parse(userBooks);
  }

  return response.data;
};

const getBook = async (bookId: number) => {
  const response = await apiClient.get(`/Book/book-details/${bookId}`);
  return response.data;
};

const addBook = async (book: any) => {
  const response = await apiClient.post(`/Book/add-book`, book);
  localStorage.removeItem("userBooks");
  return response.data;
};

const updateBook = async (book: any) => {
  const response = await apiClient.patch(`/Book/update-book`, book);
  return response.data;
};

const deleteBook = async (bookId: number) => {
  const response = await apiClient.delete(`/Book/delete-book/${bookId}`);
  const userBooks = localStorage.getItem("userBooks");
  if (userBooks && userBooks !== "undefined") {
    const books = JSON.parse(userBooks);
    const updatedBooks = books.filter((book: any) => book.id !== bookId);
    localStorage.setItem("userBooks", JSON.stringify(updatedBooks));
  }
  return response.data;
};

export default {
  getUserBooks,
  getBook,
  addBook,
  updateBook,
  deleteBook,
};
