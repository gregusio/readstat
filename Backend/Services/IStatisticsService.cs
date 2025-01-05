using Backend.DTO;

namespace Backend.Services;

public interface IStatisticsService
{
    Task<StatisticsSummaryDTO> GetStatisticsSummary(int userId);
    Task<StatisticsReadBooksDTO> GetStatisticsReadBooks(int userId);
}