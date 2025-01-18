using Backend.Models;

namespace Backend.Interfaces;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    Task<User?> GetByUsernameAsync(string email);
    Task<User?> GetByIdAsync(int id);
}