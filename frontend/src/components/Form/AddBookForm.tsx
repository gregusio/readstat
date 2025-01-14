import React, { useState } from "react";
import {
  TextField,
  Button,
  Container,
  Typography,
  Select,
  MenuItem,
  SelectChangeEvent,
  InputLabel,
  FormControl,
  Paper,
} from "@mui/material";
import Grid from "@mui/material/Grid2";
import bookService from "../../services/bookService";

interface BookData {
  title: string;
  author: string;
  additionalAuthors: string;
  isbn: string;
  isbn13: string;
  averageRating: number;
  publisher: string;
  numberOfPages: number;
  yearPublished: number;
  originalPublicationYear: number;
  myRating: number;
  exclusiveShelf: string;
  dateRead: string;
  dateAdded: string;
  myReview: string;
  readCount: number;
}

const AddBookForm: React.FC = () => {
  const [bookData, setBookData] = useState<BookData>({
    title: "",
    author: "",
    additionalAuthors: "",
    isbn: "",
    isbn13: "",
    averageRating: 0,
    publisher: "",
    numberOfPages: 0,
    yearPublished: 0,
    originalPublicationYear: 0,
    myRating: 0,
    exclusiveShelf: "",
    dateRead: new Date().toISOString(),
    dateAdded: "",
    myReview: "",
    readCount: 0,
  });

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setBookData({
      ...bookData,
      [name]: value,
    });
  };

  const handleSelectChange = (event: SelectChangeEvent<string>) => {
    const { name, value } = event.target;
    setBookData({
      ...bookData,
      [name as string]: value,
    });
    bookData.dateRead = "";
    bookData.myRating = 0;
    bookData.myReview = "";
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    bookData.dateAdded = new Date().toISOString();
    const response = await bookService.addBook(bookData);
    if (response.success) {
      alert("Book added successfully");
      localStorage.removeItem("userBooks");
    }
  };

  return (
    <Container>
      <Paper elevation={3} style={{ padding: "20px", marginTop: "20px" }}>
        <Typography variant="h4" align="center">
          Add Book
        </Typography>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={2}>
            <Grid size={6}>
              <TextField
                name="title"
                label="Title"
                value={bookData.title}
                onChange={handleChange}
                fullWidth
                margin="normal"
                title="Enter the title of the book"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                name="author"
                label="Author/s"
                value={bookData.author}
                onChange={handleChange}
                fullWidth
                margin="normal"
                title="Enter the authors of the book, if more than one separate with a comma"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                name="isbn"
                label="ISBN"
                value={bookData.isbn}
                onChange={handleChange}
                fullWidth
                margin="normal"
                title="Enter the ISBN number"
              />
            </Grid>
            <Grid size={6}>
              <TextField
                name="isbn13"
                label="ISBN13"
                value={bookData.isbn13}
                onChange={handleChange}
                fullWidth
                margin="normal"
                title="Enter the ISBN13 number"
              />
            </Grid>
            <Grid size={12}>
              <TextField
                name="publisher"
                label="Publisher"
                value={bookData.publisher}
                onChange={handleChange}
                fullWidth
                margin="normal"
                title="Enter the publisher of the book"
              />
            </Grid>
            <Grid size={3}>
              <TextField
                name="numberOfPages"
                label="Number of Pages"
                value={bookData.numberOfPages}
                onChange={handleChange}
                fullWidth
                margin="normal"
                title="Enter the number of pages in the book"
              />
            </Grid>
            <Grid size={3}>
              <TextField
                name="yearPublished"
                label="Year Published"
                value={bookData.yearPublished}
                onChange={handleChange}
                fullWidth
                margin="normal"
                title="Enter the year the book was published"
              />
            </Grid>
            <Grid size={6}>
              <FormControl fullWidth margin="normal">
                <InputLabel id="demo-simple-select-label">Shelf</InputLabel>
                <Select
                  name="exclusiveShelf"
                  label="Shelf"
                  labelId="demo-simple-select-label"
                  value={bookData.exclusiveShelf}
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
            {bookData.exclusiveShelf === "read" && (
              <>
                <Grid size={6}>
                  <TextField
                    name="myRating"
                    label="My Rating"
                    value={bookData.myRating}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                    title="Enter your rating for the book"
                  />
                </Grid>
                <Grid size={6}>
                  <TextField
                    name="dateRead"
                    label="Date Read"
                    type="date"
                    value={bookData.dateRead}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                    title="Enter the date you read the book"
                  />
                </Grid>
                <Grid size={12}>
                  <TextField
                    name="myReview"
                    label="My Review"
                    value={bookData.myReview}
                    onChange={handleChange}
                    fullWidth
                    multiline
                    rows={4}
                    margin="normal"
                    title="Enter your review of the book"
                  />
                </Grid>
              </>
            )}

            <Grid size={12}>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                fullWidth
              >
                Add Book
              </Button>
            </Grid>
          </Grid>
        </form>
      </Paper>
    </Container>
  );
};

export default AddBookForm;
