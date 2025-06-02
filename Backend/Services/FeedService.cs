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
        if (!followingUsernames.Any())
        {
            return Enumerable.Empty<UserActivityHistoryDTO>();
        }
        var feedTasks = followingIds.Select(async followingId =>
        {
            var userActivityHistory = await _feedRepository.GetUserFeedAsync(followingId);
            return userActivityHistory != null && userActivityHistory.Any()
                ? MapToDTO(userActivityHistory, followingUsernames)
                : Enumerable.Empty<UserActivityHistoryDTO>();
        });

        var userFeed = (await Task.WhenAll(feedTasks)).SelectMany(feed => feed).ToList();

        return userFeed.OrderByDescending(activity => activity.ActivityDate);
    }

    public async Task LikeActivityAsync(int userId, int activityId)
    {
        await _feedRepository.LikeActivityAsync(userId, activityId);
    }

    public async Task UnlikeActivityAsync(int userId, int activityId)
    {
        await _feedRepository.UnlikeActivityAsync(userId, activityId);
    }

    private IEnumerable<UserActivityHistoryDTO> MapToDTO(IEnumerable<UserActivityHistory> activities, IEnumerable<UserDTO> followingUsernames)
    {
        var usernameLookup = followingUsernames.ToDictionary(user => user.Id, user => user.Username);

        return activities.Select(activity => new UserActivityHistoryDTO
        {
            Id = activity.Id,
            UserId = activity.UserId,
            Username = usernameLookup.TryGetValue(activity.UserId, out var username) ? username : "Unknown",
            ActivityType = activity.ActivityType,
            ActivityDate = activity.ActivityDate,
            Description = activity.Description
        });
    }
}