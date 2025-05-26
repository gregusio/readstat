import React, { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";
import YearlyCountChart from "./YearlyCountChart";
import { Skeleton } from "@mui/material";
import { useParams } from "react-router-dom";

const YearlyReadPageCount: React.FC = () => {
  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const userId = useParams<{ userId: string }>().userId;

  useEffect(() => {
    if (!userId) {
      console.error("User ID is not available");
      setLoading(false);
      return;
    }
    statisticService.getYearlyReadPageCount(userId).then((response) => {
      setData(response);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <Skeleton variant="rectangular" width={500} height={300} />;
  }

  return <YearlyCountChart data={data} title="Yearly Read Page Count" />;
};

export default YearlyReadPageCount;
