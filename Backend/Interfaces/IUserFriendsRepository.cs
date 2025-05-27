using Backend.Models;

namespace Backend.Interfaces;

public interface IUserFriendsRepository
{
    Task<IEnumerable<UserFriends>> GetUserFriendsAsync(int userId);
    
    Task AddFriendAsync(int userId, int friendId);
    
    Task RemoveFriendAsync(int userId, int friendId);
    
    Task<bool> AreFriendsAsync(int userId, int friendId);
}