import { Box, Typography } from "@mui/material";
import Grid from "@mui/material/Grid";
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
import MostReadAuthors from "../components/Chart/MostReadAuthors";

const Statistics: React.FC = () => {
  const [hasBooks, setHasBooks] = useState(false);

  useEffect(() => {
    const checkBooks = async () => {
      const response = await bookService.getUserBooks();
      if (response.length > 0) {
        setHasBooks(true);
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
          <Grid size={{ xs: 12, sm: 12 }}>
            <MostReadAuthors />
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
