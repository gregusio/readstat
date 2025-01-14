import React, { useEffect, useState } from 'react';
import statisticService from '../../services/statisticService';
import MonthlyBookCountChart from './MonthlyCountChart';

const MonthlyReadBookCountPerYear: React.FC = () => {
  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    statisticService.getMonthlyReadBookCountPerYear().then((response) => {
      setData(response);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  return <MonthlyBookCountChart data={data} title="Monthly Read Book Count Per Year" />;
};

export default MonthlyReadBookCountPerYear;