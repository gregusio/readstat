import React, { useEffect, useState } from 'react';
import statisticService from '../../services/statisticService';
import YearlyCountChart from './YearlyCountChart';

const YearlyReadPageCount: React.FC = () => {
  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    statisticService.getYearlyReadPageCount().then((response) => {
      setData(response);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  return <YearlyCountChart data={data} title="Yearly Read Page Count" />;
};

export default YearlyReadPageCount;