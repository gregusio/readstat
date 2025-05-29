using Backend.DTO;
using Backend.Interfaces;

namespace Backend.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserDTO>> SearchUsersAsync(string searchTerm)
    {
        Console.WriteLine($"Searching for users with term: {searchTerm}");
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return Enumerable.Empty<UserDTO>();
        }

        var users = await _userRepository.SearchUsersAsync(searchTerm);
        if (users == null)
        {
            return Enumerable.Empty<UserDTO>();
        }

        return users.Select(user => new UserDTO
        {
            Id = user.Id,
            Username = user.Username
        });
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int userId)
    {
        var users = await _userRepository.GetAllUsersAsync();
        if (users == null)
        {
            return Enumerable.Empty<UserDTO>();
        }

        return users.Where(user => user.Id != userId)
                    .Select(user => new UserDTO
                    {
                        Id = user.Id,
                        Username = user.Username
                    });
    }

    public async Task<UserDTO?> GetByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username
        };
    }
}