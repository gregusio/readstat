using Backend.DTO;
using Backend.Repositories;

namespace Backend.Services;

public class StatisticsService(UserBookRecordsRepository userBookRecordsRepository) : IStatisticsService
{
    private readonly UserBookRecordsRepository _userBookRecordsRepository = userBookRecordsRepository;

    public async Task<StatisticsSummaryDTO> GetStatisticsSummary(int userId)
    {
        var totalBooks = await _userBookRecordsRepository.GetTotalBooksCountAsync(userId);
        var totalReadBooks = await _userBookRecordsRepository.GetTotalReadBooksCountAsync(userId);
        var totalReadingBooks = await _userBookRecordsRepository.GetTotalReadingBooksCountAsync(userId);
        var totalUnreadBooks = await _userBookRecordsRepository.GetTotalUnreadBooksAsync(userId);

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
        var averageRating = await _userBookRecordsRepository.GetAverageRatingAsync(userId);
        var booksWithOneStarRating = await _userBookRecordsRepository.GetBooksWithRatingAsync(userId, 1);
        var booksWithTwoStarRating = await _userBookRecordsRepository.GetBooksWithRatingAsync(userId, 2);
        var booksWithThreeStarRating = await _userBookRecordsRepository.GetBooksWithRatingAsync(userId, 3);
        var booksWithFourStarRating = await _userBookRecordsRepository.GetBooksWithRatingAsync(userId, 4);
        var booksWithFiveStarRating = await _userBookRecordsRepository.GetBooksWithRatingAsync(userId, 5);
        var mostPopularAuthorsWithBooksCount = await _userBookRecordsRepository.GetFiveMostPopularAuthorsWithBooksCountAsync(userId);

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
}