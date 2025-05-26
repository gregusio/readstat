import React, { useEffect, useState } from "react";
import statisticService from "../../services/statisticService";
import { Skeleton } from "@mui/material";
import { useParams } from "react-router-dom";

interface SummaryStats {
  totalBooks: number;
  totalReadBooks: number;
  totalReadingBooks: number;
  totalUnreadBooks: number;
}

const SummaryStatsCard: React.FC = () => {
  const [summaryStats, setSummaryStats] = useState<SummaryStats | null>(null);
  const [loading, setLoading] = useState(true);
  const userId = useParams<{ userId: string }>().userId;

  useEffect(() => {
    const fetchSummaryStats = async () => {
      try {
        if (!userId) {
          console.error("User ID is not available");
          setLoading(false);
          return;
        }
        const data = await statisticService.getSummary(userId);
        setSummaryStats(data);
        setLoading(false);
      } catch (error) {
        console.error("Failed to fetch summary stats", error);
      }
    };

    fetchSummaryStats();
  }, []);

  if (loading) {
    return <Skeleton variant="rectangular" width={500} height={300} />;
  }

  return (
    <div className="summary-stats-card">
      <h2>Summary Statistics</h2>
      {summaryStats && (
        <ul>
          <li>Total Books: {summaryStats.totalBooks}</li>
          <li>Total Read Books: {summaryStats.totalReadBooks}</li>
          <li>Total Reading Books: {summaryStats.totalReadingBooks}</li>
          <li>Total Unread Books: {summaryStats.totalUnreadBooks}</li>
        </ul>
      )}
    </div>
  );
};

export default SummaryStatsCard;
