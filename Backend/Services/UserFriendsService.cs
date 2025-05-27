using Backend.DTO;
using Backend.Interfaces;

namespace Backend.Services;

public class UserFriendsService(IUserFriendsRepository userFriendsRepository) : IUserFriendsService
{
    public async Task<IEnumerable<UserFriendsDTO>> GetUserFriendsAsync(int userId)
    {
        var friends = await userFriendsRepository.GetUserFriendsAsync(userId);
        return friends.Select(f => new UserFriendsDTO
        {
            FriendId = f.FriendId,
            FriendshipDate = f.FriendshipDate
        });
    }

    public Task AddFriendAsync(int userId, int friendId)
    {
        return userFriendsRepository.AddFriendAsync(userId, friendId);
    }

    public Task RemoveFriendAsync(int userId, int friendId)
    {
        return userFriendsRepository.RemoveFriendAsync(userId, friendId);
    }

    public Task<bool> AreFriendsAsync(int userId, int friendId)
    {
        return userFriendsRepository.AreFriendsAsync(userId, friendId);
    }
}