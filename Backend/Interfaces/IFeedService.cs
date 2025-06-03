using Backend.DTO;

namespace Backend.Interfaces;

public interface IFeedService
{
    Task<IEnumerable<UserActivityHistoryDTO>> GetUserFeedAsync(int userId);
    Task LikeActivityAsync(int userId, int activityId);
    Task UnlikeActivityAsync(int userId, int activityId);
}