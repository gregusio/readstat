using Backend.DTO;

namespace Backend.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> SearchUsersAsync(string searchTerm);
    Task<IEnumerable<UserDTO>> GetAllUsersAsync(int userId);
    Task<UserDTO?> GetByIdAsync(int userId);
}