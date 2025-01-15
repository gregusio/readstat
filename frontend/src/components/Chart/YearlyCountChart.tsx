import React, { useEffect, useState } from "react";
import { axisClasses, BarChart } from "@mui/x-charts";
import { Skeleton, Typography } from "@mui/material";

interface YearlyCountChartProps {
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

const YearlyCountChart: React.FC<YearlyCountChartProps> = ({ data, title }) => {
  const [chartData, setChartData] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const years = Object.keys(data);

    const dataset = years.map((year) => ({
      year,
      count: data[year],
    }));

    setChartData({
      labels: dataset,
      series: [{ type: "bar", dataKey: "count" }],
    });
    setLoading(false);
  }, [data]);

  if (loading) {
    return <Skeleton variant="rectangular" width={500} height={300} />;
  }

  return (
    <>
      <Typography variant="h6">{title}</Typography>
      <BarChart
        xAxis={[{ dataKey: "year", scaleType: "band", label: "Year" }]}
        series={chartData.series}
        dataset={chartData.labels}
        {...chartSetting}
      />
    </>
  );
};

export default YearlyCountChart;
