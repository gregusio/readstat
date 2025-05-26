using Backend.Models;

namespace Backend.Interfaces;

public interface IUserActivityHistoryRepository
{
    Task AddUserActivityAsync(UserActivityHistory userActivity);
    List<UserActivityHistory> GetUserActivitiesAsync(int userId);
    void DeleteUserActivityAsync(int activityId);
}