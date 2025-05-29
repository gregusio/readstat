using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserFollowingRepository(IDbContextFactory<DataContext> contextFactory) : IUserFollowingRepository
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task AddFollowingAsync(int userId, int followingId)
    {
        using var context = _contextFactory.CreateDbContext();
        var userFollowing = new UserFollowing
        {
            UserId = userId,
            FollowingId = followingId,
            FollowingDate = DateTime.UtcNow
        };
        await context.UserFollowing.AddAsync(userFollowing);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserFollowing>> GetUserFollowingAsync(int userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.UserFollowing
            .Where(uf => uf.UserId == userId)
            .ToListAsync();
    }

    public async Task RemoveFollowingAsync(int userId, int followingId)
    {
        using var context = _contextFactory.CreateDbContext();
        var userFollowing = await GetFollowingAsync(context, userId, followingId) ?? throw new InvalidOperationException("Following does not exist.");
        context.UserFollowing.Remove(userFollowing);
        await context.SaveChangesAsync();
    }

    private async Task<UserFollowing?> GetFollowingAsync(DataContext context, int userId, int followingId)
    {
        return await context.UserFollowing
            .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FollowingId == followingId);
    }
}