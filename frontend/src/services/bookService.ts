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

const addBook = async (book: any) => {
  const response = await apiClient.post(`/Book/add`, book);
  localStorage.removeItem("userBooks");
  return response.data;
};

const updateBook = async (book: any) => {
  const response = await apiClient.patch(`/Book/update`, book);
  return response.data;
};

const deleteBook = async (bookId: number) => {
  const response = await apiClient.delete(`/Book/delete/${bookId}`);
  const userBooks = localStorage.getItem("userBooks");
  if (userBooks && userBooks !== "undefined") {
    const books = JSON.parse(userBooks);
    const updatedBooks = books.filter((book: any) => book.id !== bookId);
    localStorage.setItem("userBooks", JSON.stringify(updatedBooks));
  }
  return response.data;
};

const deleteAllBooks = async () => {
  const response = await apiClient.delete(`/Book/delete/all`);
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
