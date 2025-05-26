import React, { useContext, useEffect, useState } from "react";
import bookService from "../services/bookService";
import BookCard from "../components/Card/BookCard";
import { SearchContext } from "../context/SearchContext";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  IconButton,
  Modal,
  Pagination,
  Skeleton,
  Tooltip,
  Typography,
  CircularProgress,
} from "@mui/material";
import Grid from "@mui/material/Grid";
import { Link, useNavigate } from "react-router-dom";
import InputFileUpload from "../components/Button/InputFileButton";
import HelpOutlineIcon from "@mui/icons-material/HelpOutline";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 500,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

interface Book {
  id: number;
  title: string;
  author: string;
  exclusiveShelf: string;
}

const Books: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [openModal, setOpenModal] = React.useState(false);
  const handleOpenModal = () => setOpenModal(true);
  const handleCloseModal = () => setOpenModal(false);
  const [openDialog, setOpenDialog] = React.useState(false);
  const handleOpenDialog = () => setOpenDialog(true);
  const handleCloseDialog = () => setOpenDialog(false);
  const [deleteLoading, setDeleteLoading] = useState(false);
  const booksPerPage = 24;
  const handleChange = (_event: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
  };
  const { searchQuery } = useContext(SearchContext);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchBooks = async () => {
      const response = await bookService.getUserBooks();
      setBooks(response);
      setLoading(false);
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

  const handleDeleteAll = async () => {
    setDeleteLoading(true);
    await bookService.deleteAllBooks();
    setDeleteLoading(false);
    window.location.reload();
  };

  if (loading) {
    return <Skeleton variant="rectangular" />;
  }

  return (
    <div
      style={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}
    >
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          p: 2,
          marginBottom: "10px",
        }}
      >
        <Box sx={{ display: "flex", gap: "10px" }}>
          <InputFileUpload />
          <Tooltip title="Help">
            <IconButton onClick={handleOpenModal}>
              <HelpOutlineIcon />
            </IconButton>
          </Tooltip>
        </Box>
        <Modal
          open={openModal}
          onClose={handleCloseModal}
          aria-labelledby="modal-modal-title"
          aria-describedby="modal-modal-description"
        >
          <Box sx={style}>
            <Typography id="modal-modal-title" variant="h6" component="h2">
              How to find a file to upload?
            </Typography>
            <Typography id="modal-modal-description" sx={{ mt: 2 }}>
              Go to the{" "}
              <Link to={"https://www.goodreads.com/review/import"}>
                https://www.goodreads.com/review/import
              </Link>
              , click the "Export Library" button, next after a while there will
              be a text "Your export from ...". Click on it and file will be
              downloaded. Now you can upload it here.
            </Typography>
          </Box>
        </Modal>
        <Box sx={{ display: "flex", gap: "10px" }}>
          <Button
            variant="contained"
            color="error"
            onClick={handleOpenDialog}
            disabled={deleteLoading}
          >
            {deleteLoading ? (
              <Box sx={{ display: "flex", gap: "10px" }}>
                <CircularProgress size={24} />
                Delete all
              </Box>
            ) : (
              "Delete all"
            )}
          </Button>
          <Button
            variant="contained"
            color="primary"
            onClick={() => {
              navigate("/add-book");
            }}
          >
            Add Book
          </Button>
        </Box>
        <Dialog
          open={openDialog}
          onClose={handleCloseDialog}
          aria-labelledby="alert-dialog-title"
          aria-describedby="alert-dialog-description"
        >
          <DialogTitle id="alert-dialog-title">
            {"Confirm Deletion"}
          </DialogTitle>
          <DialogContent>
            <DialogContentText id="alert-dialog-description">
              Are you sure you want to delete all books?
            </DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button onClick={handleCloseDialog} color="primary">
              Cancel
            </Button>
            <Button
              onClick={() => {
                handleDeleteAll();
                handleCloseDialog();
              }}
              color="error"
              autoFocus
            >
              Delete
            </Button>
          </DialogActions>
        </Dialog>
      </Box>
      <Grid
        container
        spacing={{ xs: 2, md: 3 }}
        columns={{ xs: 3, sm: 6, md: 9 }}
      >
        {paginatedBooks.map((book) => (
          <Grid size={3} key={book.id}>
            <BookCard
              id={book.id}
              title={book.title}
              author={book.author}
              shelf={book.exclusiveShelf}
            />
          </Grid>
        ))}
      </Grid>
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

export default Books;
