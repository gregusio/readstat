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

    public Task<bool> DeleteUserActivity(int activityId)
    {
        var activity = _userActivityHistoryRepository.GetUserActivitiesAsync(activityId);
        if (activity != null)
        {
            _userActivityHistoryRepository.DeleteUserActivityAsync(activityId);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<List<UserActivityHistoryDTO>> GetUserActivityHistory(int userId)
    {
        var userActivities = _userActivityHistoryRepository.GetUserActivitiesAsync(userId);
        var userActivityDtos = userActivities.Select(activity => new UserActivityHistoryDTO
        {
            Id = activity.Id,
            UserId = activity.UserId,
            ActivityType = activity.ActivityType,
            ActivityDate = activity.ActivityDate,
            Description = activity.Description
        }).ToList();

        return Task.FromResult(userActivityDtos);
    }
}

