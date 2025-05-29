using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task UpdateUserAsync(int id, UserProfileDTO userProfileDto);
    Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
    Task<IEnumerable<User>> GetAllUsersAsync();
}