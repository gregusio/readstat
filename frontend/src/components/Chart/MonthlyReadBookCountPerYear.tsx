import React, { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";
import { BarChart } from "@mui/x-charts";
import {
  Box,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
  Typography,
} from "@mui/material";

const chartSetting = {
  yAxis: [
    {
      label: "Count",
    },
  ],
  width: 500,
  height: 300,
};

const MonthlyReadBookCountPerYear = () => {
  const [chartData, setChartData] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [year, setYear] = useState<number>(0);
  const [years, setYears] = useState<any>([]);
  const [data, setData] = useState<any>([]);

  useEffect(() => {
    statisticService.getMonthlyReadBookCountPerYear().then((response) => {
      const data = response;
      setData(data);
      const years = Object.keys(data);
      const currYear = years[0];
      setYears(years);
      setYear(Number(currYear));
      const dataset = data[currYear].map((item: any) => item.count);

      const months = Array.from({ length: 12 }, (_, index) =>
        new Date(0, index).toLocaleString("default", { month: "long" })
      );

      setYear(Number(years[0]));

      setChartData({
        labels: months,
        series: [{ type: "bar", data: dataset }],
      });

      setLoading(false);
    });
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  const handleChange = (event: SelectChangeEvent<number>) => {
    const selectedYear = event.target.value;
    setYear(Number(selectedYear));
    const dataset = data[selectedYear].map((item: any) => item.count);
    const months = Array.from({ length: 12 }, (_, index) =>
      new Date(0, index).toLocaleString("default", { month: "long" })
    );

    setChartData({
      labels: months,
      series: [{ type: "bar", data: dataset }],
    });
  };

  return (
    <>
      <Typography variant="h6" gutterBottom>
        Monthly Read Book Count Per Year
      </Typography>
      <BarChart
        xAxis={[{ dataKey: "month", scaleType: "band", label: "Month" }]}
        series={chartData.series}
        dataset={chartData.labels.map((label: any, index: string | number) => ({
          month: label.slice(0, 3),
          value: chartData.series[0].data[index],
        }))}
        {...chartSetting}
      />

      <Box sx={{ minWidth: 120, maxWidth: 500 }}>
        <FormControl fullWidth>
          <InputLabel id="demo-simple-select-label">Year</InputLabel>
          <Select
            labelId="demo-simple-select-label"
            id="demo-simple-select"
            value={year}
            label="Year"
            onChange={handleChange}
          >
            {years.map((year: any) => (
              <MenuItem key={year} value={year}>
                {year}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Box>
    </>
  );
};

export default MonthlyReadBookCountPerYear;
