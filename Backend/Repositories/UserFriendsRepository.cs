using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserFriendsRepository(IDbContextFactory<DataContext> contextFactory) : IUserFriendsRepository
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public Task AddFriendAsync(int userId, int friendId)
    {
        using var context = _contextFactory.CreateDbContext();
        var userFriend = new UserFriends
        {
            UserId = userId,
            FriendId = friendId,
            FriendshipDate = DateTime.UtcNow
        };
        context.UserFriends.Add(userFriend);
        return context.SaveChangesAsync();
    }

    public Task<bool> AreFriendsAsync(int userId, int friendId)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.UserFriends
            .AnyAsync(uf => uf.UserId == userId && uf.FriendId == friendId);
    }

    public Task<IEnumerable<UserFriends>> GetUserFriendsAsync(int userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.UserFriends
            .Where(uf => uf.UserId == userId)
            .ToListAsync()
            .ContinueWith(task => task.Result.AsEnumerable());
    }

    public async Task RemoveFriendAsync(int userId, int friendId)
    {
        using var context = _contextFactory.CreateDbContext();
        var friendship = await GetFriendAsync(context, userId, friendId) ?? throw new InvalidOperationException("Friendship does not exist.");
        context.UserFriends.Remove(friendship);
        await context.SaveChangesAsync();
    }

    private Task<UserFriends?> GetFriendAsync(DataContext context, int userId, int friendId)
    {
        return context.UserFriends
            .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FriendId == friendId);
    }
}