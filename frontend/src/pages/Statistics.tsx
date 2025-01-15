import { Box, Typography } from "@mui/material";
import Grid from "@mui/material/Grid2";
import MonthlyReadBookCount from "../components/Chart/MonthlyReadBookCount";
import MonthlyReadPageCount from "../components/Chart/MonthlyReadPageCount";
import MonthlyAddedBookCount from "../components/Chart/MonthlyAddedBookCount";
import YearlyReadBookCount from "../components/Chart/YearlyReadBookCount";
import YearlyReadPageCount from "../components/Chart/YearlyReadPageCount";
import YearlyAddedBookCount from "../components/Chart/YearlyAddedBookCount";
import SummaryStatsCard from "../components/Card/SummaryStatsCard";
import "./Statistics.css";
import BookCount from "../components/Chart/BookCount";
import { useState, useEffect } from "react";
import bookService from "../services/bookService";

const Statistics: React.FC = () => {
  const [hasBooks, setHasBooks] = useState(false);

  useEffect(() => {
    const checkBooks = async () => {
      const books = localStorage.getItem("userBooks");
      if(books && books !== "undefined") {
        const booksArray = JSON.parse(books);
        setHasBooks(booksArray.length > 0);
      }
      else {
        const response = await bookService.getUserBooks();
        if(response) {
          setHasBooks(response.length > 0);
          localStorage.setItem("userBooks", JSON.stringify(response.data));
        }
      }
    };

    checkBooks();
  }, []);

  return (
    <Box sx={{ flexGrow: 1, p: 2 }}>
      {hasBooks ? (
        <Grid container spacing={10}>
          <Grid size={{ xs: 12, sm: 6 }}>
            <div className="center">
              <SummaryStatsCard />
            </div>
          </Grid>
          <Grid size={{ xs: 12, sm: 6 }}>
            <BookCount />
          </Grid>
          <Grid size={{ xs: 12, sm: 6 }}>
            <MonthlyReadBookCount />
          </Grid>
          <Grid size={{ xs: 12, sm: 6 }}>
            <YearlyReadBookCount />
          </Grid>
          <Grid size={{ xs: 12, sm: 6 }}>
            <MonthlyReadPageCount />
          </Grid>
          <Grid size={{ xs: 12, sm: 6 }}>
            <YearlyReadPageCount />
          </Grid>
          <Grid size={{ xs: 12, sm: 6 }}>
            <MonthlyAddedBookCount />
          </Grid>
          <Grid size={{ xs: 12, sm: 6 }}>
            <YearlyAddedBookCount />
          </Grid>
        </Grid>
      ) : (
        <Typography variant="h6" align="center">
          No books available.
        </Typography>
      )}
    </Box>
  );
};

export default Statistics;
