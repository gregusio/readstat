using Backend.DTO;

namespace Backend.Interfaces;

public interface IUserFriendsService
{   
    Task<IEnumerable<UserFriendsDTO>> GetUserFriendsAsync(int userId);
    
    Task AddFriendAsync(int userId, int friendId);
    
    Task RemoveFriendAsync(int userId, int friendId);
    
    Task<bool> AreFriendsAsync(int userId, int friendId);
}