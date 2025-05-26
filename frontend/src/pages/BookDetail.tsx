import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  SelectChangeEvent,
  MenuItem,
} from "@mui/material";
import Grid from "@mui/material/Grid";
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
  exclusiveShelf: string;
  dateRead: string | null;
  dateAdded: string | null;
  myReview: string;
  readCount: number;
}

const BookDetail: React.FC = () => {
  const { userId, bookId } = useParams();
  const [book, setBook] = useState<Book | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [editedBook, setEditedBook] = useState<Book | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    if (bookId && userId) {
      bookService.getBook(userId, parseInt(bookId)).then((response) => {
        response.dateRead = response.dateRead
          ? response.dateRead.split("T")[0]
          : "";
        response.dateAdded = response.dateAdded
          ? response.dateAdded.split("T")[0]
          : "";
        setBook(response);
        setEditedBook(response);
      });
    }
  }, [bookId]);

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
    if (book && userId) {
      bookService.deleteBook(userId, book.id).then((response) => {
        if (response.success) {
          setBook(null);
          navigate(`/${userId}/books`);
        }
      });
    }
  };

  const handleSave = () => {
    if (editedBook) {
      editedBook.dateAdded = editedBook.dateAdded
        ? new Date(editedBook.dateAdded).toISOString()
        : null;
      editedBook.dateRead = editedBook.dateRead
        ? new Date(editedBook.dateRead).toISOString()
        : null;
      if (editedBook.exclusiveShelf !== "read") {
        editedBook.myRating = 0;
        editedBook.dateRead = null;
        editedBook.myReview = "";
      }
      if (!userId) {
        console.error("User ID is not defined");
        return;
      }
      bookService.updateBook(userId, editedBook).then((response) => {
        if (response.success) {
          editedBook.dateRead = editedBook.dateRead
            ? editedBook.dateRead.split("T")[0]
            : "";
          editedBook.dateAdded = editedBook.dateAdded
            ? editedBook.dateAdded.split("T")[0]
            : "";
          setBook(editedBook);
          setIsEditing(false);
          const storedBooks = localStorage.getItem("userBooks");
          if (storedBooks && storedBooks !== "undefined") {
            const books = JSON.parse(storedBooks);
            const updatedBooks = books.map((b: Book) => {
              if (b.id === editedBook.id) {
                return {
                  ...b,
                  title:
                    editedBook.title !== b.title ? editedBook.title : b.title,
                  author:
                    editedBook.author !== b.author
                      ? editedBook.author
                      : b.author,
                  shelf:
                    editedBook.exclusiveShelf !== b.exclusiveShelf
                      ? editedBook.exclusiveShelf
                      : b.exclusiveShelf,
                };
              }
              return b;
            });
            localStorage.setItem("userBooks", JSON.stringify(updatedBooks));
          }
        }
      });
    }
  };

  const handleSelectChange = (e: SelectChangeEvent<string>) => {
    if (editedBook) {
      setEditedBook({
        ...editedBook,
        exclusiveShelf: e.target.value as string,
      });
    }
  };

  const calculateTimeDifference = (startDate: string, endDate: string) => {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const diffTime = Math.abs(end.getTime() - start.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays;
  };

  const getTimeWaiting = () => {
    const currentDate = new Date().toISOString().split("T")[0];
    if (book?.dateRead) {
      return book.dateRead && book.dateAdded
        ? calculateTimeDifference(book.dateAdded, book.dateRead)
        : 0;
    } else {
      return book?.dateAdded
        ? calculateTimeDifference(book.dateAdded, currentDate)
        : 0;
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
            <Grid size={6}>
              <FormControl fullWidth margin="normal">
                <InputLabel id="demo-simple-select-label">Shelf</InputLabel>
                <Select
                  name="exclusiveShelf"
                  label="Shelf"
                  labelId="demo-simple-select-label"
                  value={editedBook?.exclusiveShelf}
                  defaultValue={editedBook?.exclusiveShelf}
                  onChange={handleSelectChange}
                  fullWidth
                  title="Select the shelf you want to add the book to"
                >
                  <MenuItem value="read" title="Read">
                    Read
                  </MenuItem>
                  <MenuItem value="currently-reading" title="Currently Reading">
                    Currently Reading
                  </MenuItem>
                  <MenuItem value="to-read" title="To Read">
                    To Read
                  </MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid size={3}>
              <TextField
                label="Date Added"
                name="dateAdded"
                value={editedBook?.dateAdded}
                onChange={handleInputChange}
                fullWidth
                margin="normal"
              />
            </Grid>
            {editedBook?.exclusiveShelf === "read" && (
              <>
                <Grid size={6}>
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
                    label="Date Read"
                    name="dateRead"
                    value={editedBook?.dateRead}
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
              </>
            )}
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
              Shelf: {book.exclusiveShelf}
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
            <Typography variant="body2" component="p">
              {book.dateRead
                ? `Time taken to read: ${getTimeWaiting()} days`
                : `Time waiting to be read: ${getTimeWaiting()} days`}
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
