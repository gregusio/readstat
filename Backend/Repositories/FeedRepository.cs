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

    public async Task LikeActivityAsync(int userId, int activityId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var activity = await context.UserActivityHistories
            .FirstOrDefaultAsync(a => a.Id == activityId);

        await context.Likes.AddAsync(new Like
        {
            UserId = userId,
            ActivityId = activityId
        });

        if (activity != null)
        {
            activity.Likes++;
            await context.SaveChangesAsync();
        }
    }

    public async Task UnlikeActivityAsync(int userId, int activityId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var activity = await context.UserActivityHistories
            .FirstOrDefaultAsync(a => a.Id == activityId);

        var like = await context.Likes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.ActivityId == activityId);

        if (like != null)
        {
            context.Likes.Remove(like);
        }

        if (activity != null && activity.Likes > 0)
        {
            activity.Likes--;
            await context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> IsActivityLikedAsync(int activityId, int userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        Console.WriteLine($"Checking if activity {activityId} is liked by user {userId}");
        return await context.Likes.AnyAsync(l => l.ActivityId == activityId && l.UserId == userId);
    }
}