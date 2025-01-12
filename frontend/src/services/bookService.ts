import apiClient from "./apiClient";

const getUserBooks = async () => {
    const response = await apiClient.get(`/Book/user-books`);
    console.log(response.data);
    return response.data;
}

const getBook = async (bookId: number) => {
    const response = await apiClient.get(`/Book/book-details/${bookId}`);
    return response.data;
}

const addBook = async (book: any) => {
    const response = await apiClient.post(`/Book/add-book`, book);
    return response.data;
}

const updateBook = async (book: any) => {
    const response = await apiClient.patch(`/Book/update-book`, book);
    return response.data;
}

const deleteBook = async (bookId: number) => {
    const response = await apiClient.delete(`/Book/delete-book/${bookId}`);
    return response.data;
}

export default {
    getUserBooks,
    getBook,
    addBook,
    updateBook,
    deleteBook,
};
