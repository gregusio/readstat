import React, { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";
import MonthlyCountChart from "./MonthlyCountChart";
import { Skeleton } from "@mui/material";

const MonthlyReadBookCount: React.FC = () => {
  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    statisticService.getMonthlyReadBookCountPerYear().then((response) => {
      setData(response);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <Skeleton variant="rectangular" width={500} height={300} />;
  }

  return (
    <MonthlyCountChart data={data} title="Monthly Read Book Count Per Year" />
  );
};

export default MonthlyReadBookCount;