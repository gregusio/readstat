using Backend.Models;

namespace Backend.Interfaces;

public interface IUserFollowingRepository
{
    Task<IEnumerable<UserFollowing>> GetUserFollowingAsync(int userId);
    
    Task AddFollowingAsync(int userId, int followingId);
    
    Task RemoveFollowingAsync(int userId, int followingId);
}