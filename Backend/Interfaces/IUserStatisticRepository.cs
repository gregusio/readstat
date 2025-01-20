using Backend.DTO;

namespace Backend.Interfaces;

public interface IUserStatisticRepository
{
    Task<int> GetTotalBooksCountAsync(int userId);
    Task<int> GetTotalReadBooksCountAsync(int userId);
    Task<int> GetTotalReadingBooksCountAsync(int userId);
    Task<int> GetTotalUnreadBooksAsync(int userId);
    Task<double> GetAverageRatingAsync(int userId);
    Task<List<BookBasicInfoDTO>> GetBooksWithRatingAsync(int userId, int rating);
    Task<Dictionary<string, int>> GetNumberOfBooksReadPerYearAndMonthAsync(int userId);
    Task<Dictionary<string, int>> GetNumberOfPagesReadPerYearAndMonthAsync(int userId);
    Task<string> GetMostProductiveYearAsync(int userId);
    Task<string> GetMostProductiveYearAndMonthAsync(int userId);
    Task<double> GetAveragePagesPerBookAsync(int userId);
    Task<int> GetTotalPagesReadAsync(int userId);
    Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyReadBookCountPerYearAsync(int userId);
    Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyReadPageCountPerYearAsync(int userId);
    Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyAddedBookCountPerYearAsync(int userId);
    Task<Dictionary<int, int>> GetYearlyReadBookCountAsync(int userId);
    Task<Dictionary<int, int>> GetYearlyReadPageCountAsync(int userId);
    Task<Dictionary<int, int>> GetYearlyAddedBookCountAsync(int userId);
    Task<Dictionary<string, int>> GetMostReadAuthorsAsync(int userId);
}