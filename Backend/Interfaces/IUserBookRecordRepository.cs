using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces;

public interface IUserBookRecordRepository
{
    Task<IEnumerable<UserBookRecord>> GetAllForUserAsync(int userId);
    Task<UserBookRecord?> GetByIdAsync(int id);
    Task<UserBookRecord?> GetByUserIdAndBookIdAsync(int userId, int recordId);
    Task AddRangeAsync(IEnumerable<UserBookRecord> records);
    Task AddAsync(UserBookRecord record);
    Task UpdateAsync(UserBookRecord record);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}