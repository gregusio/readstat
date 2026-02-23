using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserActivityHistoryRepository(IDbContextFactory<DataContext> contextFactory) : IUserActivityHistoryRepository
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory = contextFactory;

    public async Task AddUserActivityAsync(UserActivityHistory userActivity)
    {
        await using var context = _dbContextFactory.CreateDbContext();
        await context.UserActivityHistories.AddAsync(userActivity);
        await context.SaveChangesAsync();
    }

    public async Task<List<UserActivityHistory>> GetUserActivitiesAsync(int userId)
    {
        await using var context = _dbContextFactory.CreateDbContext();
        return await context.UserActivityHistories
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task DeleteUserActivityAsync(int activityId)
    {
        await using var context = _dbContextFactory.CreateDbContext();
        var activity = await context.UserActivityHistories.FindAsync(activityId);
        if (activity != null)
        {
            context.UserActivityHistories.Remove(activity);
            await context.SaveChangesAsync();
        }
    }
}
