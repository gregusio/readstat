using Backend.DTO;
using Backend.Interfaces;

namespace Backend.Services;

public class UserFollowingService(IUserFollowingRepository userFollowingRepository, IUserRepository userRepository) : IUserFollowingService
{
    private readonly IUserFollowingRepository _userFollowingRepository = userFollowingRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserFollowingDTO>> GetUserFollowingAsync(int userId)
    {
        var userFollowings = await _userFollowingRepository.GetUserFollowingAsync(userId);
        if (userFollowings == null || !userFollowings.Any())
        {
            return Enumerable.Empty<UserFollowingDTO>();
        }

        var followingUserIds = userFollowings.Select(uf => uf.FollowingId).ToList();
        var followingUsers = await _userRepository.GetByIdsAsync(followingUserIds);
        var followingUsersUsernames = followingUsers.ToDictionary(u => u.Id, u => u.Username);
        if (followingUsersUsernames.Count == 0)
        {
            return userFollowings.Select(uf => new UserFollowingDTO
            {
                FollowingId = uf.FollowingId,
                FollowingUsername = "Unknown",
                FollowingDate = uf.FollowingDate
            });
        }
        
        return userFollowings.Select(uf => new UserFollowingDTO
        {
            FollowingId = uf.FollowingId,
            FollowingUsername = followingUsersUsernames.TryGetValue(uf.FollowingId, out var username) ? username : "Unknown",
            FollowingDate = uf.FollowingDate
        });
    }

    public Task AddFollowingAsync(int userId, int followingId)
    {
        return _userFollowingRepository.AddFollowingAsync(userId, followingId);
    }

    public Task RemoveFollowingAsync(int userId, int followingId)
    {
        return _userFollowingRepository.RemoveFollowingAsync(userId, followingId);
    }
}