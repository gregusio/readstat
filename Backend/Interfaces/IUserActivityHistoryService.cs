using Backend.DTO;

namespace Backend.Services;

public interface IUserActivityHistoryService
{
    Task<List<UserActivityHistoryDTO>> GetUserActivityHistory(int userId);
    Task<UserActivityHistoryDTO> AddUserActivity(UserActivityHistoryDTO userActivityDto);
    Task<bool> DeleteUserActivity(int activityId);
}