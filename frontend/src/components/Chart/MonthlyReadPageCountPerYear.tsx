import React, { useEffect, useState } from 'react';
import statisticService from '../../services/statisticService';
import MonthlyCountChart from './MonthlyCountChart';

const MonthlyReadPageCountPerYear = () => {
  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    statisticService.getMonthlyReadPageCountPerYear().then((response) => {
      setData(response);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  return <MonthlyCountChart data={data} title="Monthly Added Book Count Per Year" />;
};

export default MonthlyReadPageCountPerYear;