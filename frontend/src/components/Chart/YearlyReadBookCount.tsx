import React, { useEffect, useState } from 'react';
import statisticService from '../../services/statisticService';
import YearlyCountChart from './YearlyCountChart';

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
    return <div>Loading...</div>;
  }

  return <YearlyCountChart data={data} title="Read Book Count Per Year" />;
};

export default YearlyReadBookCount;