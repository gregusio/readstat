using Backend.DTO;
using Backend.Interfaces;
using Backend.Repositories;

namespace Backend.Services;

public class ProfileService(IUserActivityHistoryService userActivityHistoryService, IUserRepository userRepository) : IProfileService
{
    private readonly IUserActivityHistoryService _userActivityHistoryService = userActivityHistoryService;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserProfileDTO> GetUserProfile(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new Exception("User not found");
        var userActivityHistory = await _userActivityHistoryService.GetUserActivityHistory(userId);
        var userProfileDto = new UserProfileDTO
        {
            Id = user.Id,
            Username = user.Username,
            AvatarUrl = string.Empty, // TODO: avatarUrl should be added to the User entity
            Bio = string.Empty, // TODO: bio should be added to the User entity
            UserActivityHistory = userActivityHistory
        };
        return userProfileDto;
    }

    public async Task<UserProfileDTO> UpdateUserProfile(int userId, UserProfileDTO userProfileDto)
    {
        await _userRepository.UpdateUserAsync(userId, userProfileDto);

        return await GetUserProfile(userId);
    }
}

    