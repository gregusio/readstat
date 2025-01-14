import React, { useContext, useEffect, useState } from "react";
import bookService from "../services/bookService";
import BookCard from "../components/Card/BookCard";
import { SearchContext } from "../context/SearchContext";
import { Pagination, Skeleton, Stack } from "@mui/material";

interface Book {
  id: number;
  title: string;
  author: string;
  exclusiveShelf: string;
}

const UserBooks: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const booksPerPage = 25;
  const handleChange = (_event: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
  };
  const { searchQuery } = useContext(SearchContext);

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        const cachedBooks = localStorage.getItem("userBooks");
        if (cachedBooks) {
          setBooks(JSON.parse(cachedBooks));
          setLoading(false);
        } else {
          const books = await bookService.getUserBooks();
          setBooks(books);
          localStorage.setItem("userBooks", JSON.stringify(books));
        }
      } catch (error) {
        console.error("Error fetching books:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchBooks();
  }, []);

  const filteredBooks = books.filter(
    (book) =>
      book.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.author.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const paginatedBooks = filteredBooks.slice(
    (page - 1) * booksPerPage,
    page * booksPerPage
  );

  if (loading) {
    return <Skeleton variant="rectangular" />;
  }

  return (
    <div
      style={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}
    >
      <Stack
        spacing={2}
        style={{ flex: "1 0 auto", overflowY: "auto", paddingBottom: "60px" }}
      >
        {paginatedBooks.map((book) => (
          <BookCard
            key={book.id}
            id={book.id}
            title={book.title}
            author={book.author}
            shelf={book.exclusiveShelf}
          />
        ))}
      </Stack>
      <div
        style={{
          position: "fixed",
          bottom: 0,
          left: 0,
          width: "100%",
          padding: "10px 0",
          boxShadow: "0 -2px 5px rgba(0,0,0,0.1)",
        }}
      >
        <Pagination
          count={Math.ceil(filteredBooks.length / booksPerPage)}
          page={page}
          onChange={handleChange}
          style={{ display: "flex", justifyContent: "center" }}
        />
      </div>
    </div>
  );
};

export default UserBooks;
