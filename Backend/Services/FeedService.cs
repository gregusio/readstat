using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class FeedService(IFeedRepository feedRepository, IUserFollowingService userFollowingService, IUserService userService) : IFeedService
{
    private readonly IFeedRepository _feedRepository = feedRepository;
    private readonly IUserFollowingService _userFollowingService = userFollowingService;
    private readonly IUserService _userService = userService;

    public async Task<IEnumerable<UserActivityHistoryDTO>> GetUserFeedAsync(int userId)
    {
        var userFollowing = await _userFollowingService.GetUserFollowingAsync(userId);
        var followingIds = userFollowing.Select(following => following.FollowingId).ToList();
        var followingUsernames = await _userService.GetByIdsAsync(followingIds);
        if (followingUsernames == null || !followingUsernames.Any())
        {
            return Enumerable.Empty<UserActivityHistoryDTO>();
        }
        var userFeed = new List<UserActivityHistoryDTO>();
        foreach (var followingId in followingIds)
        {
            var userActivityHistory = await _feedRepository.GetUserFeedAsync(followingId);
            if (userActivityHistory != null && userActivityHistory.Any())
            {
                userFeed.AddRange(MapToDTO(userActivityHistory, followingUsernames));
            }
        }

        return userFeed.OrderByDescending(activity => activity.ActivityDate);
    }

    private IEnumerable<UserActivityHistoryDTO> MapToDTO(IEnumerable<UserActivityHistory> activities, IEnumerable<UserDTO> followingUsernames)
    {
        return activities.Select(activity => new UserActivityHistoryDTO
        {
            Id = activity.Id,
            UserId = activity.UserId,
            Username = followingUsernames.FirstOrDefault(user => user.Id == activity.UserId)?.Username ?? "Unknown",
            ActivityType = activity.ActivityType,
            ActivityDate = activity.ActivityDate,
            Description = activity.Description
        });
    }
}