import React, { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";

interface SummaryStats {
  totalBooks: number;
  totalReadBooks: number;
  totalReadingBooks: number;
  totalUnreadBooks: number;
}

const SummaryStatsCard: React.FC = () => {
  const [summaryStats, setSummaryStats] = useState<SummaryStats | null>(null);

  useEffect(() => {
    const fetchSummaryStats = async () => {
      try {
        const data = await statisticService.getSummary();
        setSummaryStats(data);
      } catch (error) {
        console.error("Failed to fetch summary stats", error);
      }
    };

    fetchSummaryStats();
  }, []);

  if (!summaryStats) {
    return <div>Loading...</div>;
  }

  return (
    <div className="summary-stats-card">
      <h2>Summary Statistics</h2>
      <ul>
        <li>Total Books: {summaryStats.totalBooks}</li>
        <li>Total Read Books: {summaryStats.totalReadBooks}</li>
        <li>Total Reading Books: {summaryStats.totalReadingBooks}</li>
        <li>Total Unread Books: {summaryStats.totalUnreadBooks}</li>
      </ul>
    </div>
  );
};

export default SummaryStatsCard;
