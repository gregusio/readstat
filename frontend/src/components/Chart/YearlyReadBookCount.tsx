import React, { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";
import YearlyCountChart from "./YearlyCountChart";
import { Skeleton } from "@mui/material";

const YearlyReadBookCount: React.FC = () => {
  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    statisticService.getYearlyReadBookCount().then((response) => {
      setData(response);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <Skeleton variant="rectangular" width={500} height={300} />;
  }

  return <YearlyCountChart data={data} title="Yearly Read Book Count" />;
};

export default YearlyReadBookCount;
