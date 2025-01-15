import React, { useEffect, useState } from "react";
import { axisClasses, BarChart } from "@mui/x-charts";
import { Skeleton, Typography } from "@mui/material";

interface MonthlyCountChartProps {
  data: any;
  title: string;
}

const chartSetting = {
  yAxis: [
    {
      label: "Count",
    },
  ],
  height: 300,
  margin: { left: 60, right: 20 },
  sx: {
    [`.${axisClasses.left} .${axisClasses.label}`]: {
      transform: "translate(-20px, 0)",
    },
  },
};

const MonthlyCountChart: React.FC<MonthlyCountChartProps> = ({
  data,
  title,
}) => {
  const [chartData, setChartData] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [year, setYear] = useState<number>(0);
  const [years, setYears] = useState<any>([]);

  useEffect(() => {
    const years = Object.keys(data);
    const currYear = years[0];
    setYears(years);
    setYear(Number(currYear));
    updateChartData(currYear, data);
    setLoading(false);
  }, [data]);

  const updateChartData = (selectedYear: string, data: any) => {
    const dataset = data[selectedYear].map((item: any) => item.count);
    const months = data[selectedYear].map((item: any) => item.month);

    setChartData({
      labels: months,
      series: [{ type: "bar", data: dataset }],
    });
  };

  const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedYear = event.target.value;
    setYear(Number(selectedYear));
    updateChartData(selectedYear, data);
  };

  if (loading) {
    return <Skeleton variant="rectangular" width={500} height={300} />;
  }

  return (
    <>
      <div style={{ display: "flex", gap: "20px", alignItems: "center" }}>
        <Typography variant="h6">
          {title}
        </Typography>
        <select value={year} onChange={handleChange}>
          {years.map((year: string) => (
        <option key={year} value={year}>
          {year}
        </option>
          ))}
        </select>
      </div>
      <BarChart
        xAxis={[{ dataKey: "month", scaleType: "band", label: "Month" }]}
        series={chartData.series}
        dataset={chartData.labels.map((label: any, index: string | number) => ({
          month: label.slice(0, 3),
          value: chartData.series[0].data[index],
        }))}
        {...chartSetting}
      />
    </>
  );
};

export default MonthlyCountChart;
