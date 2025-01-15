import React, { useEffect, useState } from "react";
import { motion } from "framer-motion";
import Button from "@mui/material/Button";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import confetti from "canvas-confetti";
import bookService from "../services/bookService";

interface Book {
  title: string;
  author: string;
  exclusiveShelf: string;
}

const DrawBook: React.FC = () => {
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  const [isRolling, setIsRolling] = useState(false);
  const [books, setBooks] = useState<Book[]>([]);

  useEffect(() => {
    const books = localStorage.getItem("userBooks");
    if (books) {
      const filteredBooks = JSON.parse(books).filter(
        (book: Book) => book.exclusiveShelf === "to-read"
      );
      setBooks(filteredBooks);
    } else {
      bookService.getUserBooks().then((books) => {
        localStorage.setItem("userBooks", JSON.stringify(books));
        const filteredBooks = books.filter(
          (book: Book) => book.exclusiveShelf === "to-read"
        );
        setBooks(filteredBooks);
      });
    }
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
      ) : selectedBook ? (
        <Card style={{ maxWidth: "400px", margin: "0 auto" }}>
          <CardContent>
            <Typography variant="h5">{selectedBook.title}</Typography>
            <Typography variant="body1">{selectedBook.author}</Typography>
          </CardContent>
        </Card>
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
