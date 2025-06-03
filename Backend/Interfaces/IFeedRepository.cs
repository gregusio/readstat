using Backend.Models;

namespace Backend.Interfaces;

public interface IFeedRepository
{
    Task<IEnumerable<UserActivityHistory>> GetUserFeedAsync(int userId);
    Task LikeActivityAsync(int userId, int activityId);
    Task UnlikeActivityAsync(int userId, int activityId);
    Task<bool> IsActivityLikedAsync(int activityId, int userId);
}