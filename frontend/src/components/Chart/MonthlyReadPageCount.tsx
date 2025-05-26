import React, { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";
import MonthlyCountChart from "./MonthlyCountChart";
import { Skeleton } from "@mui/material";
import { useParams } from "react-router-dom";

const MonthlyReadPageCount: React.FC = () => {
  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const userId = useParams<{ userId: string }>().userId;

  useEffect(() => {
    if (!userId) {
      console.error("User ID is not available");
      setLoading(false);
      return;
    }
    statisticService.getMonthlyReadPageCountPerYear(userId).then((response) => {
      setData(response);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <Skeleton variant="rectangular" width={500} height={300} />;
  }

  return (
    <MonthlyCountChart data={data} title="Monthly Read Page Count Per Year" />
  );
};

export default MonthlyReadPageCount;
