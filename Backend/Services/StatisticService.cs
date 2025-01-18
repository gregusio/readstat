using Backend.DTO;
using Backend.Interfaces;

namespace Backend.Services;

public class StatisticService(IUserStatisticRepository userStatisticsRepository) : IStatisticService
{
    private readonly IUserStatisticRepository _userStatisticsRepository = userStatisticsRepository;

    public async Task<StatisticsSummaryDTO> GetStatisticsSummary(int userId)
    {
        var totalBooks = await _userStatisticsRepository.GetTotalBooksCountAsync(userId);
        var totalReadBooks = await _userStatisticsRepository.GetTotalReadBooksCountAsync(userId);
        var totalReadingBooks = await _userStatisticsRepository.GetTotalReadingBooksCountAsync(userId);
        var totalUnreadBooks = await _userStatisticsRepository.GetTotalUnreadBooksAsync(userId);

        return new StatisticsSummaryDTO
        {
            TotalBooks = totalBooks,
            TotalReadBooks = totalReadBooks,
            TotalReadingBooks = totalReadingBooks,
            TotalUnreadBooks = totalUnreadBooks
        };
    }

    public async Task<StatisticsReadBooksDTO> GetStatisticsReadBooks(int userId)
    {
        var averageRating = await _userStatisticsRepository.GetAverageRatingAsync(userId);
        var booksWithOneStarRating = await _userStatisticsRepository.GetBooksWithRatingAsync(userId, 1);
        var booksWithTwoStarRating = await _userStatisticsRepository.GetBooksWithRatingAsync(userId, 2);
        var booksWithThreeStarRating = await _userStatisticsRepository.GetBooksWithRatingAsync(userId, 3);
        var booksWithFourStarRating = await _userStatisticsRepository.GetBooksWithRatingAsync(userId, 4);
        var booksWithFiveStarRating = await _userStatisticsRepository.GetBooksWithRatingAsync(userId, 5);
        var mostPopularAuthorsWithBooksCount = await _userStatisticsRepository.GetFiveMostPopularAuthorsWithBooksCountAsync(userId);

        return new StatisticsReadBooksDTO
        {
            AverageRating = averageRating,
            BooksWithOneStarRating = booksWithOneStarRating,
            BooksWithTwoStarRating = booksWithTwoStarRating,
            BooksWithThreeStarRating = booksWithThreeStarRating,
            BooksWithFourStarRating = booksWithFourStarRating,
            BooksWithFiveStarRating = booksWithFiveStarRating,
            MostFivePopularAuthorsWithBooksCount = mostPopularAuthorsWithBooksCount
        };
    }

    public async Task<StatisticsReadingProgressDTO> GetStatisticsReadingProgress(int userId)
    {
        var numberOfBooksReadPerYearAndMonth = await _userStatisticsRepository.GetNumberOfBooksReadPerYearAndMonthAsync(userId);
        var numberOfPagesReadPerYearAndMonth = await _userStatisticsRepository.GetNumberOfPagesReadPerYearAndMonthAsync(userId);
        var mostProductiveYear = await _userStatisticsRepository.GetMostProductiveYearAsync(userId);
        var mostProductiveMonth = await _userStatisticsRepository.GetMostProductiveYearAndMonthAsync(userId);
        var averagePagesPerBook = await _userStatisticsRepository.GetAveragePagesPerBookAsync(userId);
        var totalPagesRead = await _userStatisticsRepository.GetTotalPagesReadAsync(userId);

        return new StatisticsReadingProgressDTO
        {
            BooksReadPerMonth = numberOfBooksReadPerYearAndMonth,
            PagesReadPerMonth = numberOfPagesReadPerYearAndMonth,
            MostProductiveYear = mostProductiveYear,
            MostProductiveMonth = mostProductiveMonth,
            AveragePagesPerBook = averagePagesPerBook,
            TotalPagesRead = totalPagesRead
        };
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetStatisticsMonthlyReadBookCountPerYear(int userId)
    {
        return await _userStatisticsRepository.GetMonthlyReadBookCountPerYearAsync(userId);
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetStatisticsMonthlyReadPageCountPerYear(int userId)
    {
        return await _userStatisticsRepository.GetMonthlyReadPageCountPerYearAsync(userId);
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetStatisticsMonthlyAddedBookCountPerYear(int userId)
    {
        return await _userStatisticsRepository.GetMonthlyAddedBookCountPerYearAsync(userId);
    }

    public async Task<Dictionary<int, int>> GetStatisticsYearlyReadBookCountPerYear(int userId)
    {
        return await _userStatisticsRepository.GetYearlyReadBookCountAsync(userId);
    }

    public async Task<Dictionary<int, int>> GetStatisticsYearlyReadPageCountPerYear(int userId)
    {
        return await _userStatisticsRepository.GetYearlyReadPageCountAsync(userId);
    }

    public async Task<Dictionary<int, int>> GetStatisticsYearlyAddedBookCountPerYear(int userId)
    {
        return await _userStatisticsRepository.GetYearlyAddedBookCountAsync(userId);
    }
}