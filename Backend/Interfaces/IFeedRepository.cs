using Backend.Models;

namespace Backend.Interfaces;

public interface IFeedRepository
{
    Task<IEnumerable<UserActivityHistory>> GetUserFeedAsync(int userId);
}