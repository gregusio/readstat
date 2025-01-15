import { Box } from "@mui/material";
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

const Statistics: React.FC = () => {
  return (
    <Box sx={{ flexGrow: 1, p: 2 }}>
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
    </Box>
  );
};

export default Statistics;
