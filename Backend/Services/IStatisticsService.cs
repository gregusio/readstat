using Backend.DTO;

namespace Backend.Services;

public interface IStatisticsService
{
    Task<StatisticsSummaryDTO> GetStatisticsSummary(int userId);
    Task<StatisticsReadBooksDTO> GetStatisticsReadBooks(int userId);
    Task<StatisticsReadingProgressDTO> GetStatisticsReadingProgress(int userId);
    Task<Dictionary<int, List<MonthlyStats>>> GetStatisticsMonthlyReadBookCountPerYear(int userId);
    Task<Dictionary<int, List<MonthlyStats>>> GetStatisticsMonthlyReadPageCountPerYear(int userId);
    Task<Dictionary<int, List<MonthlyStats>>> GetStatisticsMonthlyAddedBookCountPerYear(int userId);
    Task<Dictionary<int, int>> GetStatisticsYearlyReadBookCountPerYear(int userId);
}