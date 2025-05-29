using Backend.DTO;

namespace Backend.Interfaces;

public interface IUserFollowingService
{   
    Task<IEnumerable<UserFollowingDTO>> GetUserFollowingAsync(int userId);
    
    Task AddFollowingAsync(int userId, int followingId);
    
    Task RemoveFollowingAsync(int userId, int followingId);
}