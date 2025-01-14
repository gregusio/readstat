using System.Globalization;
using Backend.Data;
using Backend.DTO;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserBookRecordsRepository(IDbContextFactory<DataContext> contextFactory)
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<IEnumerable<UserBookRecord>> GetAllForUserAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId)
            .ToListAsync();
    }

    public async Task<UserBookRecord?> GetByIdAsync(int id)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .FirstOrDefaultAsync(ubr => ubr.Id == id);
    }

    public async Task<UserBookRecord?> GetByUserIdAndBookIdAsync(int userId, int recordId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .FirstOrDefaultAsync(ubr => ubr.UserId == userId && ubr.Id == recordId);
    }

    public async Task<int> GetTotalBooksCountAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId)
            .CountAsync();
    }

    public async Task<int> GetTotalReadBooksCountAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.ExclusiveShelf == "read") // TODO add some status enum
            .CountAsync();
    }

    public async Task<int> GetTotalReadingBooksCountAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.ExclusiveShelf == "currently-reading") // TODO add some status enum
            .CountAsync();
    }

    public async Task<int> GetTotalUnreadBooksAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.ExclusiveShelf == "to-read") // TODO add some status enum
            .CountAsync();
    }

    public async Task<double> GetAverageRatingAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.MyRating != 0)
            .AverageAsync(ubr => ubr.MyRating);
    }

    public async Task<List<BookBasicInfoDTO>> GetBooksWithRatingAsync(int userId, int rating)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecordsWithRating = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.MyRating == rating)
            .ToListAsync();

        var booksBasicInfoWithRating = new List<BookBasicInfoDTO>();
        foreach (var record in userBooksRecordsWithRating)
        {
            var book = await _context.Books.FindAsync(record.BookId);
            booksBasicInfoWithRating.Add(new BookBasicInfoDTO
            {
                Id = record.BookId,
                Title = record.UserTitle ?? book!.Title,
                Author = record.UserAuthor ?? book!.Author,
                ExclusiveShelf = record.ExclusiveShelf,
            });
        }

        return booksBasicInfoWithRating;
    }

    public async Task<Dictionary<string, int>> GetFiveMostPopularAuthorsWithBooksCountAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId)
            .ToListAsync();

        var authorsBooksCount = new Dictionary<string, int>();
        foreach (var record in userBooksRecords)
        {
            var book = await _context.Books.FindAsync(record.BookId);
            if (book != null && (!string.IsNullOrEmpty(book.Author) || !string.IsNullOrEmpty(record.UserAuthor)))
            {
                var author = record.UserAuthor ?? book.Author;
                if (!string.IsNullOrEmpty(author) && authorsBooksCount.ContainsKey(author))
                {
                    authorsBooksCount[author]++;
                }
                else if (!string.IsNullOrEmpty(author))
                {
                    authorsBooksCount[author] = 1;
                }

            }
        }

        return authorsBooksCount
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public async Task<Dictionary<string, int>> GetNumberOfBooksReadPerYearAndMonthAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var booksReadPerYearAndMonth = new Dictionary<string, int>();
        foreach (var record in userBooksRecords)
        {
            var year = record.DateRead!.Value.Year;
            var month = record.DateRead!.Value.Month;
            if (booksReadPerYearAndMonth.ContainsKey($"{year}-{month}"))
            {
                booksReadPerYearAndMonth[$"{year}-{month}"]++;
            }
            else
            {
                booksReadPerYearAndMonth[$"{year}-{month}"] = 1;
            }
        }

        return booksReadPerYearAndMonth;
    }

    public async Task<Dictionary<string, int>> GetNumberOfPagesReadPerYearAndMonthAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var pagesReadPerYearAndMonth = new Dictionary<string, int>();
        foreach (var record in userBooksRecords)
        {
            var book = await _context.Books.FindAsync(record.BookId);
            var year = record.DateRead!.Value.Year;
            var month = record.DateRead!.Value.Month;
            if (pagesReadPerYearAndMonth.ContainsKey($"{year}-{month}"))
            {
                pagesReadPerYearAndMonth[$"{year}-{month}"] += record.UserNumberOfPages ?? book!.NumberOfPages ?? 0;
            }
            else
            {
                pagesReadPerYearAndMonth[$"{year}-{month}"] = record.UserNumberOfPages ?? book!.NumberOfPages ?? 0;
            }
        }

        return pagesReadPerYearAndMonth;
    }

    public async Task<string> GetMostProductiveYearAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var booksReadPerYear = new Dictionary<int, int>();
        foreach (var record in userBooksRecords)
        {
            var year = record.DateRead!.Value.Year;
            if (booksReadPerYear.ContainsKey(year))
            {
                booksReadPerYear[year]++;
            }
            else
            {
                booksReadPerYear[year] = 1;
            }
        }

        return booksReadPerYear
            .OrderByDescending(kvp => kvp.Value)
            .FirstOrDefault()
            .Key
            .ToString();
    }

    public async Task<string> GetMostProductiveYearAndMonthAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var booksReadPerYearAndMonth = new Dictionary<(int, int), int>();
        foreach (var record in userBooksRecords)
        {
            var year = record.DateRead!.Value.Year;
            var month = record.DateRead!.Value.Month;
            if (booksReadPerYearAndMonth.ContainsKey((year, month)))
            {
                booksReadPerYearAndMonth[(year, month)]++;
            }
            else
            {
                booksReadPerYearAndMonth[(year, month)] = 1;
            }
        }

        var mostProductiveYearAndMonth = booksReadPerYearAndMonth
            .OrderByDescending(kvp => kvp.Value)
            .FirstOrDefault();

        return mostProductiveYearAndMonth.Key.Item1 + "-" + mostProductiveYearAndMonth.Key.Item2;
    }

    public async Task<double> GetAveragePagesPerBookAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var totalPagesRead = 0;
        foreach (var record in userBooksRecords)
        {
            var book = await _context.Books.FindAsync(record.BookId);
            totalPagesRead += record.UserNumberOfPages ?? book!.NumberOfPages ?? 0;
        }

        return (double)totalPagesRead / userBooksRecords.Count;
    }

    public async Task<int> GetTotalPagesReadAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var totalPagesRead = 0;
        foreach (var record in userBooksRecords)
        {
            var book = await _context.Books.FindAsync(record.BookId);
            totalPagesRead += record.UserNumberOfPages ?? book!.NumberOfPages ?? 0;
        }

        return totalPagesRead;
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyReadBookCountPerYearAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var monthlyReadBookCountPerYear = new Dictionary<int, List<MonthlyStats>>();
        var months = DateTimeFormatInfo.CurrentInfo!.MonthNames.Take(12).ToArray();

        var years = userBooksRecords.Select(record => record.DateRead!.Value.Year).Distinct().ToList();
        years.Sort();

        foreach (var year in years)
        {
            monthlyReadBookCountPerYear[year] = months.Select(month => new MonthlyStats
            {
                Month = month,
                Count = 0
            }).ToList();
        }

        foreach (var record in userBooksRecords)
        {
            var year = record.DateRead!.Value.Year;
            var month = record.DateRead.Value.Month;
            var monthlyStats = monthlyReadBookCountPerYear[year];
            var stats = monthlyStats.First(ms => ms.Month == months[month - 1]);
            stats.Count++;
        }

        return monthlyReadBookCountPerYear;
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyReadPageCountPerYearAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var monthlyReadPageCountPerYear = new Dictionary<int, List<MonthlyStats>>();
        var months = DateTimeFormatInfo.CurrentInfo!.MonthNames.Take(12).ToArray();

        var years = userBooksRecords.Select(record => record.DateRead!.Value.Year).Distinct().ToList();
        years.Sort();

        foreach (var year in years)
        {
            monthlyReadPageCountPerYear[year] = months.Select(month => new MonthlyStats
            {
                Month = month,
                Count = 0
            }).ToList();
        }

        foreach (var record in userBooksRecords)
        {
            var book = await _context.Books.FindAsync(record.BookId);
            var year = record.DateRead!.Value.Year;
            var month = record.DateRead.Value.Month;
            var monthlyStats = monthlyReadPageCountPerYear[year];
            var stats = monthlyStats.First(ms => ms.Month == months[month - 1]);
            stats.Count += record.UserNumberOfPages ?? book!.NumberOfPages ?? 0;
        }

        return monthlyReadPageCountPerYear;
    }

    public async Task<Dictionary<int, List<MonthlyStats>>> GetMonthlyAddedBookCountPerYearAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateAdded != null)
            .ToListAsync();

        var monthlyAddedBookCountPerYear = new Dictionary<int, List<MonthlyStats>>();
        var months = DateTimeFormatInfo.CurrentInfo!.MonthNames.Take(12).ToArray();

        var years = userBooksRecords.Select(record => record.DateAdded!.Value.Year).Distinct().ToList();
        years.Sort();

        foreach (var year in years)
        {
            monthlyAddedBookCountPerYear[year] = months.Select(month => new MonthlyStats
            {
                Month = month,
                Count = 0
            }).ToList();
        }

        foreach (var record in userBooksRecords)
        {
            var year = record.DateAdded!.Value.Year;
            var month = record.DateAdded.Value.Month;
            var monthlyStats = monthlyAddedBookCountPerYear[year];
            var stats = monthlyStats.First(ms => ms.Month == months[month - 1]);
            stats.Count++;
        }

        return monthlyAddedBookCountPerYear;
    }

    public async Task<Dictionary<int, int>> GetYearlyReadBookCountAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var yearlyReadBookCountPerYear = new Dictionary<int, int>();

        var years = userBooksRecords.Select(record => record.DateRead!.Value.Year).Distinct().ToList();
        years.Sort();

        foreach (var year in years)
        {
            yearlyReadBookCountPerYear[year] = userBooksRecords
                .Count(record => record.DateRead!.Value.Year == year);
        }

        return yearlyReadBookCountPerYear;
    }

    public async Task<Dictionary<int, int>> GetYearlyReadPageCountAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateRead != null)
            .ToListAsync();

        var books = userBooksRecords.Select(record => record.BookId).Distinct().ToList();
        var booksWithPages = await _context.Books.Where(book => books.Contains(book.Id)).ToListAsync();

        var yearlyReadPageCountPerYear = new Dictionary<int, int>();

        var years = userBooksRecords.Select(record => record.DateRead!.Value.Year).Distinct().ToList();
        years.Sort();

        foreach (var year in years)
        {
            yearlyReadPageCountPerYear[year] = userBooksRecords
                .Where(record => record.DateRead!.Value.Year == year)
                .Sum(record => record.UserNumberOfPages ?? booksWithPages.First(book => book.Id == record.BookId).NumberOfPages ?? 0);
        }

        return yearlyReadPageCountPerYear;
    }

    public async Task<Dictionary<int, int>> GetYearlyAddedBookCountAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var userBooksRecords = await _context.UserBookRecords
            .Where(ubr => ubr.UserId == userId && ubr.DateAdded != null)
            .ToListAsync();

        var yearlyAddedBookCountPerYear = new Dictionary<int, int>();

        var years = userBooksRecords.Select(record => record.DateAdded!.Value.Year).Distinct().ToList();
        years.Sort();

        foreach (var year in years)
        {
            yearlyAddedBookCountPerYear[year] = userBooksRecords
                .Count(record => record.DateAdded!.Value.Year == year);
        }

        return yearlyAddedBookCountPerYear;
    }

    public async Task AddRangeAsync(IEnumerable<UserBookRecord> records)
    {
        await using var _context = _contextFactory.CreateDbContext();
        await _context.UserBookRecords.AddRangeAsync(records);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(UserBookRecord record)
    {
        await using var _context = _contextFactory.CreateDbContext();
        await _context.UserBookRecords.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserBookRecord record)
    {
        await using var _context = _contextFactory.CreateDbContext();
        _context.UserBookRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var record = await GetByIdAsync(id);
        if (record != null)
        {
            await using var _context = _contextFactory.CreateDbContext();
            _context.UserBookRecords.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords.AnyAsync(ubr => ubr.Id == id);
    }
}
