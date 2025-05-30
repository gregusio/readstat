using Backend.DTO;

namespace Backend.Interfaces;

public interface IFeedService
{
    Task<IEnumerable<UserActivityHistoryDTO>> GetUserFeedAsync(int userId);
}