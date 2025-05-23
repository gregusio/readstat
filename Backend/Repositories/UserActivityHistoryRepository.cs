using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserActivityHistoryRepository(IDbContextFactory<DataContext> contextFactory)
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory = contextFactory;

    public async Task AddUserActivityAsync(UserActivityHistory userActivity)
    {
        await using var _context = _dbContextFactory.CreateDbContext();
        await _context.UserActivityHistories.AddAsync(userActivity);
        await _context.SaveChangesAsync();
    }

    public List<UserActivityHistory> GetUserActivitiesAsync(int userId)
    {
        using var _context = _dbContextFactory.CreateDbContext();
        return _context.UserActivityHistories
            .Where(x => x.UserId == userId)
            .ToList();
    }

    public void DeleteUserActivityAsync(int activityId)
    {
        using var _context = _dbContextFactory.CreateDbContext();
        var activity = _context.UserActivityHistories.Find(activityId);
        if (activity != null)
        {
            _context.UserActivityHistories.Remove(activity);
            _context.SaveChanges();
        }
    }
}