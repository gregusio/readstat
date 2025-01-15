import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
} from "@mui/material";
import Grid from "@mui/material/Grid2";
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
  publishYear: string;
  originalPublishYear: string;
  numberOfPages: number;
  myRating: number;
  shelf: string;
  dateRead: string;
  dateAdded: string;
  myReview: string;
  readCount: number;
}

const BookDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [book, setBook] = useState<Book | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [editedBook, setEditedBook] = useState<Book | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    if (id) {
      bookService.getBook(parseInt(id)).then((response) => {
        setBook(response);
        setEditedBook(response);
      });
    }
  }, [id]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (editedBook) {
      setEditedBook({
        ...editedBook,
        [e.target.name]: e.target.value,
      });
    }
  };

  const handleEdit = () => {
    setIsEditing(true);
    setEditedBook(book);
  };

  const handleDelete = () => {
    if (book) {
      
      bookService.deleteBook(book.id).then((response) => {
        if (response.success) {
          setBook(null);
          navigate("/books");
        }
      });
    }
  };

  const handleSave = () => {
    if (editedBook) {
      bookService.updateBook(editedBook).then((response) => {
        if (response.success) {
          setBook(editedBook);
          setIsEditing(false);
        }
      });
    }
  };

  if (!book) {
    return <div>Loading...</div>;
  }

  return (
    <Card>
      <CardContent>
        {isEditing ? (
          <Grid container spacing={1}>
            <Grid size={6}>
              <TextField
                label="Title"
                name="title"
                value={editedBook?.title || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                label="Author"
                name="author"
                value={editedBook?.author || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={12}>
              <TextField
                label="Additional Authors"
                name="additionalAuthors"
                value={editedBook?.additionalAuthors || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={4}>
              <TextField
                label="ISBN"
                name="isbn"
                value={editedBook?.isbn || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={4}>
              <TextField
                label="ISBN-13"
                name="isbn13"
                value={editedBook?.isbn13 || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={4}>
              <TextField
                label="Average Rating"
                name="averageRating"
                value={editedBook?.averageRating || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={12}>
              <TextField
                label="Publisher"
                name="publisher"
                value={editedBook?.publisher || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                label="Publish Year"
                name="publishYear"
                value={editedBook?.publishYear || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                label="Original Publish Year"
                name="originalPublishYear"
                value={editedBook?.originalPublishYear || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={3}>
              <TextField
                label="Number of Pages"
                name="numberOfPages"
                value={editedBook?.numberOfPages || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={3}>
              <TextField
                label="My Rating"
                name="myRating"
                value={editedBook?.myRating || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                label="Shelf"
                name="shelf"
                value={editedBook?.shelf || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                label="Date Read"
                name="dateRead"
                value={editedBook?.dateRead || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                label="Date Added"
                name="dateAdded"
                value={editedBook?.dateAdded || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            <Grid size={12}>
              <TextField
                label="My Review"
                name="myReview"
                value={editedBook?.myReview || ""}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
                multiline
                rows={4}
              />
            </Grid>
            <Button onClick={handleSave} color="primary" variant="contained">
              Save
            </Button>
            <Button
              onClick={() => setIsEditing(false)}
              color="secondary"
              variant="contained"
            >
              Cancel
            </Button>
          </Grid>
        ) : (
          <>
            <Typography variant="h5" component="h2">
              {book.title}
            </Typography>
            <Typography color="textSecondary">{book.author}</Typography>
            <Typography variant="body2" component="p">
              Additional Authors: {book.additionalAuthors}
            </Typography>
            <Typography variant="body2" component="p">
              ISBN: {book.isbn}
            </Typography>
            <Typography variant="body2" component="p">
              ISBN-13: {book.isbn13}
            </Typography>
            <Typography variant="body2" component="p">
              Average Rating: {book.averageRating}
            </Typography>
            <Typography variant="body2" component="p">
              Publisher: {book.publisher}
            </Typography>
            <Typography variant="body2" component="p">
              Publish Year: {book.publishYear}
            </Typography>
            <Typography variant="body2" component="p">
              Original Publish Year: {book.originalPublishYear}
            </Typography>
            <Typography variant="body2" component="p">
              Number of Pages: {book.numberOfPages}
            </Typography>
            <Typography variant="body2" component="p">
              My Rating: {book.myRating}
            </Typography>
            <Typography variant="body2" component="p">
              Shelf: {book.shelf}
            </Typography>
            <Typography variant="body2" component="p">
              Date Read: {book.dateRead}
            </Typography>
            <Typography variant="body2" component="p">
              Date Added: {book.dateAdded}
            </Typography>
            <Typography variant="body2" component="p">
              My Review: {book.myReview}
            </Typography>
            <Typography variant="body2" component="p">
              Read Count: {book.readCount}
            </Typography>
            <Button onClick={handleEdit} color="primary" variant="contained">
              Edit
            </Button>
            <Button
              onClick={handleDelete}
              color="secondary"
              variant="contained"
            >
              Delete
            </Button>
          </>
        )}
      </CardContent>
    </Card>
  );
};

export default BookDetail;
