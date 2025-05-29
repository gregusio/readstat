using Backend.DTO;
using Backend.Interfaces;

namespace Backend.Services;

public class UserFollowingService(IUserFollowingRepository userFollowingRepository) : IUserFollowingService
{
    private readonly IUserFollowingRepository _userFollowingRepository = userFollowingRepository;

    public async Task<IEnumerable<UserFollowingDTO>> GetUserFollowingAsync(int userId)
    {
        var followingList = await _userFollowingRepository.GetUserFollowingAsync(userId);
        return followingList.Select(f => new UserFollowingDTO
        {
            FollowingId = f.FollowingId,
            FollowingDate = f.FollowingDate
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