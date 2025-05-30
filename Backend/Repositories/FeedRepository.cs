using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class FeedRepository(IDbContextFactory<DataContext> contextFactory) : IFeedRepository
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<IEnumerable<UserActivityHistory>> GetUserFeedAsync(int userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.UserActivityHistories
            .Where(activity => activity.UserId == userId)
            .OrderByDescending(activity => activity.ActivityDate)
            .ToListAsync();
    }
}