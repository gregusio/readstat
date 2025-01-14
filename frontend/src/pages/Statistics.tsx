import { Box } from "@mui/material";
import Grid from "@mui/material/Grid2";
import MonthlyReadBookCountPerYear from "../components/Chart/MonthlyReadBookCountPerYear";
import MonthlyReadPageCountPerYear from "../components/Chart/MonthlyReadPageCountPerYear";
import MonthlyAddedBookCountPerYear from "../components/Chart/MonthlyAddedBookCountPerYear";
import YearlyReadBookCount from "../components/Chart/YearlyReadBookCount";
import YearlyReadPageCount from "../components/Chart/YearlyReadPageCount";
import YearlyAddedBookCount from "../components/Chart/YearlyAddedBookCount";
import SummaryStatsCard from "../components/Card/SummaryStatsCard";
import "./Statistics.css";

const Statistics: React.FC = () => {
  return (
    <Box sx={{ flexGrow: 1, p: 2 }}>
      <Grid container spacing={10}>
        <Grid size={6}>
          <div className="center">
            <SummaryStatsCard />
          </div>
        </Grid>
        <Grid size={6}>
          <MonthlyReadBookCountPerYear />
        </Grid>
        <Grid size={6}>
          <MonthlyReadPageCountPerYear />
        </Grid>
        <Grid size={6}>
          <MonthlyAddedBookCountPerYear />
        </Grid>
        <Grid size={6}>
          <YearlyReadBookCount />
        </Grid>
        <Grid size={6}>
          <YearlyReadPageCount />
        </Grid>
        <Grid size={6}>
          <YearlyAddedBookCount />
        </Grid>
      </Grid>
    </Box>
  );
};

export default Statistics;
