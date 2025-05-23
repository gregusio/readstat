using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserBookRecordRepository(IDbContextFactory<DataContext> contextFactory) : IUserBookRecordRepository
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

    public async Task AddRangeAsync(IEnumerable<UserBookRecord> records)
    {
        await using var _context = _contextFactory.CreateDbContext();
        await _context.UserBookRecords.AddRangeAsync(records);
        await _context.UserActivityHistories.AddAsync(new UserActivityHistory
        {
            UserId = records.First().UserId,
            ActivityType = "Upload",
            Description = string.Format("Uploaded {0} book records", records.Count()),
            ActivityDate = DateTime.UtcNow,
        });
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(UserBookRecord record)
    {
        await using var _context = _contextFactory.CreateDbContext();
        await _context.UserBookRecords.AddAsync(record);
        await _context.UserActivityHistories.AddAsync(new UserActivityHistory
        {
            UserId = record.UserId,
            ActivityType = "Add",
            Description = string.Format("Added a book \"{0}\"", record.UserTitle),
            ActivityDate = DateTime.UtcNow,
        });
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
