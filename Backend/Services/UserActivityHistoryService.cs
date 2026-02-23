using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class UserActivityHistoryService(IUserActivityHistoryRepository userActivityHistoryRepository) : IUserActivityHistoryService
{
    private readonly IUserActivityHistoryRepository _userActivityHistoryRepository = userActivityHistoryRepository;

    public async Task<UserActivityHistoryDTO> AddUserActivity(UserActivityHistoryDTO userActivityDto)
    {
        var userActivity = new UserActivityHistory
        {
            UserId = userActivityDto.UserId,
            ActivityType = userActivityDto.ActivityType,
            ActivityDate = userActivityDto.ActivityDate,
            Description = userActivityDto.Description
        };

        await _userActivityHistoryRepository.AddUserActivityAsync(userActivity);

        return new UserActivityHistoryDTO
        {
            Id = userActivity.Id,
            UserId = userActivity.UserId,
            ActivityType = userActivity.ActivityType,
            ActivityDate = userActivity.ActivityDate,
            Description = userActivity.Description
        };
    }

    public async Task<bool> DeleteUserActivity(int activityId)
    {
        var activities = await _userActivityHistoryRepository.GetUserActivitiesAsync(activityId);
        if (activities != null)
        {
            await _userActivityHistoryRepository.DeleteUserActivityAsync(activityId);
            return true;
        }
        return false;
    }

    public async Task<List<UserActivityHistoryDTO>> GetUserActivityHistory(int userId)
    {
        var userActivities = await _userActivityHistoryRepository.GetUserActivitiesAsync(userId);

        return userActivities.Select(activity => new UserActivityHistoryDTO
        {
            Id = activity.Id,
            UserId = activity.UserId,
            ActivityType = activity.ActivityType,
            ActivityDate = activity.ActivityDate,
            Description = activity.Description
        }).ToList();
    }
}
