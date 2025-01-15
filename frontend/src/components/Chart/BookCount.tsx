import { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";
import { Skeleton, Typography } from "@mui/material";
import { PieChart } from "@mui/x-charts";

const BookCount: React.FC = () => {
  const [chartData, setChartData] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    statisticService.getSummary().then((response) => {
      const { totalReadBooks, totalUnreadBooks } = response;
      setChartData([
        { label: "Read", value: totalReadBooks },
        { label: "Unread", value: totalUnreadBooks, color: "gray" },
      ]);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <Skeleton variant="rectangular" width={500} height={300} />;
  }

  return (
    <>
      <Typography variant="h6">Book Count</Typography>
      <PieChart
        series={[
          {
            data: chartData,
            highlightScope: { fade: "global", highlight: "item" },
            faded: { innerRadius: 30, additionalRadius: -30, color: "gray" },
          },
        ]}
        height={200}
      />
    </>
  );
};

export default BookCount;
