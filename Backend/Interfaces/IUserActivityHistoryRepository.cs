using Backend.Models;

namespace Backend.Interfaces;

public interface IUserActivityHistoryRepository
{
    Task AddUserActivityAsync(UserActivityHistory userActivity);
    Task<List<UserActivityHistory>> GetUserActivitiesAsync(int userId);
    Task DeleteUserActivityAsync(int activityId);
}
