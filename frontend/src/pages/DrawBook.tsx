import React, { useEffect, useState } from "react";
import { motion } from "framer-motion";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import confetti from "canvas-confetti";
import bookService from "../services/bookService";
import BookCard from "../components/Card/BookCard";
import { useParams } from "react-router-dom";

interface Book {
  id: number;
  title: string;
  author: string;
  exclusiveShelf: string;
}

const DrawBook: React.FC = () => {
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  const [isRolling, setIsRolling] = useState(false);
  const [books, setBooks] = useState<Book[]>([]);
  const userId = useParams<{ userId: string }>().userId;

  useEffect(() => {
    const fetchBooks = async () => {
      if (!userId) {
        console.error("User ID is not available");
        return;
      }
      const response = await bookService.getUserBooks(userId);
      const filteredBooks = response.filter(
        (book: Book) => book.exclusiveShelf === "to-read"
      );
      setBooks(filteredBooks);
    };

    fetchBooks();
  }, []);

  const handleDraw = () => {
    setIsRolling(true);
    setTimeout(() => {
      const randomIndex = Math.floor(Math.random() * books.length);
      setSelectedBook(books[randomIndex]);
      setIsRolling(false);

      confetti({
        particleCount: 100,
        spread: 70,
        origin: { y: 0.6 },
      });
    }, 2000);
  };

  return (
    <div style={{ textAlign: "center", marginTop: "50px" }}>
      {isRolling ? (
        <motion.div
          animate={{ rotate: 360 }}
          transition={{ duration: 0.5, repeat: Infinity }}
          style={{
            fontSize: "3rem",
            fontWeight: "bold",
            marginBottom: "20px",
          }}
        >
          <img
            src="/book_icon.png"
            alt="logo"
            style={{ width: "200px", height: "200px" }}
          />
        </motion.div>
      ) : selectedBook && userId ? (
        <div style={{ display: "flex", justifyContent: "center" }}>
          <BookCard
            userId={userId}
            id={selectedBook.id}
            title={selectedBook.title}
            author={selectedBook.author}
            shelf={selectedBook.exclusiveShelf}
          />
        </div>
      ) : (
        <Typography variant="h6">Click the button to draw a book!</Typography>
      )}

      <Button
        onClick={handleDraw}
        variant="contained"
        color="primary"
        style={{ marginTop: "20px" }}
        disabled={isRolling}
      >
        Draw a Book
      </Button>
    </div>
  );
};

export default DrawBook;
