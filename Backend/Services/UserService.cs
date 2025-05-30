using Backend.DTO;
using Backend.Interfaces;

namespace Backend.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserDTO>> SearchUsersAsync(string searchTerm)
    {
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

    public async Task<IEnumerable<UserDTO>> GetByIdsAsync(IEnumerable<int> userIds)
    {
        if (userIds == null || !userIds.Any())
        {
            return Enumerable.Empty<UserDTO>();
        }

        var users = await Task.WhenAll(userIds.Select(id => _userRepository.GetByIdAsync(id))); //TODO change to GetByIdsAsync if implemented 
        return users
            .Where(user => user != null)
            .Select(user => new UserDTO
            {
                Id = user!.Id,
                Username = user.Username
            });
    }
}