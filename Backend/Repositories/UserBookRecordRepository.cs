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

    public async Task<UserBookRecord?> GetByUserIdAndBookIdAsync(int userId, int bookId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.UserBookRecords
            .FirstOrDefaultAsync(ubr => ubr.UserId == userId && ubr.BookId == bookId);
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
                if (authorsBooksCount.ContainsKey(record.UserAuthor ?? book.Author))
                {
                    authorsBooksCount[record.UserAuthor ?? book.Author]++;
                }
                else
                {
                    authorsBooksCount[record.UserAuthor ?? book.Author] = 1;
                }
                
            }
        }

        return authorsBooksCount
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
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
