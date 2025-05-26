import apiClient from "./apiClient";

const getUserBooks = async (userId: string) => {
  // const userBooks = localStorage.getItem("userBooks");
  
  // if (userBooks && userBooks !== "undefined") {
  //   return JSON.parse(userBooks);
  // }
  // TODO: Implement caching logic if needed

  const response = await apiClient.get(`/Book/user/${userId}`);
  localStorage.setItem("userBooks", JSON.stringify(response.data));

  return response.data;
};

const getBook = async (userId: string ,bookId: number) => {
  const response = await apiClient.get(`/Book/user/${userId}/book/${bookId}`);
  return response.data;
};

const addBook = async (userId: string, book: any) => {
  const response = await apiClient.post(`/Book/user/${userId}/book/add`, book);
  localStorage.removeItem("userBooks");
  return response.data;
};

const updateBook = async (userId: string, book: any) => {
  const response = await apiClient.patch(`/Book/user/${userId}/book/update`, book);
  return response.data;
};

const deleteBook = async (userId: string, bookId: number) => {
  const response = await apiClient.delete(`/Book/user/${userId}/book/delete/${bookId}`);
  const userBooks = localStorage.getItem("userBooks");
  if (userBooks && userBooks !== "undefined") {
    const books = JSON.parse(userBooks);
    const updatedBooks = books.filter((book: any) => book.id !== bookId);
    localStorage.setItem("userBooks", JSON.stringify(updatedBooks));
  }
  return response.data;
};

const deleteAllBooks = async (userId: string) => {
  const response = await apiClient.delete(`/Book/user/${userId}/book/delete/all`);
  localStorage.removeItem("userBooks");
  return response.data;
}

export default {
  getUserBooks,
  getBook,
  addBook,
  updateBook,
  deleteBook,
  deleteAllBooks,
};
