import React, { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";
import { BarChart } from "@mui/x-charts";

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
      const dataset = data[currYear];

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

  const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedYear = event.target.value;
    setYear(Number(selectedYear));
    const dataset = data[selectedYear];
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
      <select value={year} onChange={handleChange}>
        {years.map((year: string) => (
          <option key={year} value={year}>
            {year}
          </option>
        ))}
      </select>
      {chartData && (
        <BarChart
          xAxis={[{ dataKey: "month", scaleType: "band" }]}
          series={chartData.series}
          dataset={chartData.labels.map(
            (label: any, index: string | number) => ({
              month: label.slice(0, 3),
              value: chartData.series[0].data[index],
            })
          )}
          {...chartSetting}
        />
      )}
    </>
  );
};

export default MonthlyReadBookCountPerYear;
