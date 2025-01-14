import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import bookService from "../services/bookService";

interface Book {
  id: number;
  title: string;
  author: string;
  additionalAuthors: string;
  isbn: string;
  isbn13: string;
  averageRating: number;
  publisher: string;
  publishDate: string;
  originalPublishDate: string;
  numberOfPages: number;
  myRating: number;
  shelf: string;
  dateRead: string;
  dateAdded: string;
  myReview: string;
  readCount: number;
}

const BookDetail = () => {
  const { id } = useParams<{ id: string }>();

  const [book, setBook] = useState<Book | null>(null);

  useEffect(() => {
    if (id) {
      bookService.getBook(parseInt(id)).then((response) => {
        setBook(response);
      });
    }
  }, [id]);

  if (!book) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>{book.title}</h1>
      <p>{book.author}</p>
      <p>{book.additionalAuthors}</p>
      <p>{book.isbn}</p>
      <p>{book.isbn13}</p>
      <p>{book.averageRating}</p>
      <p>{book.publisher}</p>
      <p>{book.publishDate}</p>
      <p>{book.originalPublishDate}</p>
      <p>{book.numberOfPages}</p>
      <p>{book.myRating}</p>
      <p>{book.shelf}</p>
      <p>{book.dateRead}</p>
      <p>{book.dateAdded}</p>
      <p>{book.myReview}</p>
      <p>{book.readCount}</p>
    </div>
  );
};

export default BookDetail;
