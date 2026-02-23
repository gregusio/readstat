using System.Globalization;
using Backend.Data;
using Backend.DTO;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserStatisticRepository(IDbContextFactory<DataContext> contextFactory) : IUserStatisticRepository
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<int> GetTotalBooksCountAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId)
            .CountAsync();
    }

    public async Task<int> GetTotalReadBooksCountAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.ExclusiveShelf == "read")
            .CountAsync();
    }

    public async Task<int> GetTotalReadingBooksCountAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.ExclusiveShelf == "currently-reading")
            .CountAsync();
    }

    public async Task<int> GetTotalUnreadBooksAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.ExclusiveShelf == "to-read")
            .CountAsync();
    }

    public async Task<double> GetAverageRatingAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.MyRating != 0)
            .AverageAsync(ubr => ubr.MyRating);
    }

    public async Task<List<BookBasicInfoDTO>> GetBooksWithRatingAsync(int userId, int rating)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecordsWithRating = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.MyRating == rating)
            .ToListAsync();

        var bookIds = userBooksRecordsWithRating.Select(ubr => ubr.BookId).Distinct();
        var books = await context.Books
            .Where(b => bookIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id);

        return userBooksRecordsWithRating.Select(record => new BookBasicInfoDTO
        {
            Id = record.BookId,
            Title = record.UserTitle ?? books[record.BookId].Title,
            Author = record.UserAuthor ?? books[record.BookId].Author,
            ExclusiveShelf = record.ExclusiveShelf,
        }).ToList();
    }

    public async Task<Dictionary<string, int>> GetMostReadAuthorsAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var bookIds = userBooksRecords.Select(ubr => ubr.BookId).Distinct();
        var books = await context.Books
            .Where(b => bookIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id);

        var authorsBooksCount = new Dictionary<string, int>();
        foreach (var record in userBooksRecords)
        {
            books.TryGetValue(record.BookId, out var book);
            var author = record.UserAuthor ?? book?.Author;
            if (string.IsNullOrEmpty(author))
                continue;

            authorsBooksCount[author] = authorsBooksCount.TryGetValue(author, out var count) ? count + 1 : 1;
        }

        return authorsBooksCount
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public async Task<Dictionary<string, int>> GetNumberOfBooksReadPerYearAndMonthAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var booksReadPerYearAndMonth = new Dictionary<string, int>();
        foreach (var record in userBooksRecords)
        {
            var key = $"{record.DateRead!.Value.Year}-{record.DateRead.Value.Month}";
            booksReadPerYearAndMonth[key] = booksReadPerYearAndMonth.TryGetValue(key, out var count) ? count + 1 : 1;
        }

        return booksReadPerYearAndMonth;
    }

    public async Task<Dictionary<string, int>> GetNumberOfPagesReadPerYearAndMonthAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var bookIds = userBooksRecords.Select(ubr => ubr.BookId).Distinct();
        var books = await context.Books
            .Where(b => bookIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id);

        var pagesReadPerYearAndMonth = new Dictionary<string, int>();
        foreach (var record in userBooksRecords)
        {
            books.TryGetValue(record.BookId, out var book);
            var key = $"{record.DateRead!.Value.Year}-{record.DateRead.Value.Month}";
            var pages = record.UserNumberOfPages ?? book?.NumberOfPages ?? 0;
            pagesReadPerYearAndMonth[key] = pagesReadPerYearAndMonth.TryGetValue(key, out var count) ? count + pages : pages;
        }

        return pagesReadPerYearAndMonth;
    }

    public async Task<string> GetMostProductiveYearAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        return userBooksRecords
            .GroupBy(record => record.DateRead!.Value.Year)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key
            .ToString() ?? string.Empty;
    }

    public async Task<string> GetMostProductiveYearAndMonthAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var mostProductive = userBooksRecords
            .GroupBy(record => new { record.DateRead!.Value.Year, record.DateRead.Value.Month })
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key;

        return mostProductive != null ? $"{mostProductive.Year}-{mostProductive.Month}" : string.Empty;
    }

    public async Task<double> GetAveragePagesPerBookAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        if (userBooksRecords.Count == 0)
            return 0;

        var bookIds = userBooksRecords.Select(ubr => ubr.BookId).Distinct();
        var books = await context.Books
            .Where(b => bookIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id);

        var totalPages = userBooksRecords.Sum(record =>
        {
            books.TryGetValue(record.BookId, out var book);
            return record.UserNumberOfPages ?? book?.NumberOfPages ?? 0;
        });

        return (double)totalPages / userBooksRecords.Count;
    }

    public async Task<int> GetTotalPagesReadAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var bookIds = userBooksRecords.Select(ubr => ubr.BookId).Distinct();
        var books = await context.Books
            .Where(b => bookIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id);

        return userBooksRecords.Sum(record =>
        {
            books.TryGetValue(record.BookId, out var book);
            return record.UserNumberOfPages ?? book?.NumberOfPages ?? 0;
        });
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyReadBookCountPerYearAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var months = DateTimeFormatInfo.CurrentInfo!.MonthNames.Take(12).ToArray();
        var years = userBooksRecords.Select(record => record.DateRead!.Value.Year).Distinct().OrderBy(y => y);

        var result = years.ToDictionary(
            year => year,
            year => months.Select(month => new MonthlyStats { Month = month, Count = 0 }).ToList()
        );

        foreach (var record in userBooksRecords)
        {
            var year = record.DateRead!.Value.Year;
            var month = record.DateRead.Value.Month;
            result[year].First(ms => ms.Month == months[month - 1]).Count++;
        }

        return result;
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyReadPageCountPerYearAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var bookIds = userBooksRecords.Select(ubr => ubr.BookId).Distinct();
        var books = await context.Books
            .Where(b => bookIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id);

        var months = DateTimeFormatInfo.CurrentInfo!.MonthNames.Take(12).ToArray();
        var years = userBooksRecords.Select(record => record.DateRead!.Value.Year).Distinct().OrderBy(y => y);

        var result = years.ToDictionary(
            year => year,
            year => months.Select(month => new MonthlyStats { Month = month, Count = 0 }).ToList()
        );

        foreach (var record in userBooksRecords)
        {
            books.TryGetValue(record.BookId, out var book);
            var year = record.DateRead!.Value.Year;
            var month = record.DateRead.Value.Month;
            result[year].First(ms => ms.Month == months[month - 1]).Count += record.UserNumberOfPages ?? book?.NumberOfPages ?? 0;
        }

        return result;
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyAddedBookCountPerYearAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateAdded != null)
            .ToListAsync();

        var months = DateTimeFormatInfo.CurrentInfo!.MonthNames.Take(12).ToArray();
        var years = userBooksRecords.Select(record => record.DateAdded!.Value.Year).Distinct().OrderBy(y => y);

        var result = years.ToDictionary(
            year => year,
            year => months.Select(month => new MonthlyStats { Month = month, Count = 0 }).ToList()
        );

        foreach (var record in userBooksRecords)
        {
            var year = record.DateAdded!.Value.Year;
            var month = record.DateAdded.Value.Month;
            result[year].First(ms => ms.Month == months[month - 1]).Count++;
        }

        return result;
    }

    public async Task<Dictionary<int, int>> GetYearlyReadBookCountAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        return userBooksRecords
            .GroupBy(record => record.DateRead!.Value.Year)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<int, int>> GetYearlyReadPageCountAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var bookIds = userBooksRecords.Select(record => record.BookId).Distinct();
        var books = await context.Books
            .Where(b => bookIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id);

        return userBooksRecords
            .GroupBy(record => record.DateRead!.Value.Year)
            .OrderBy(g => g.Key)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(record =>
                {
                    books.TryGetValue(record.BookId, out var book);
                    return record.UserNumberOfPages ?? book?.NumberOfPages ?? 0;
                })
            );
    }

    public async Task<Dictionary<int, int>> GetYearlyAddedBookCountAsync(int userId)
    {
        await using var context = _contextFactory.CreateDbContext();
        var userBooksRecords = await context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateAdded != null)
            .ToListAsync();

        return userBooksRecords
            .GroupBy(record => record.DateAdded!.Value.Year)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}
