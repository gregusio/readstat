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
        var users = await _userRepository.GetByIdsAsync([userId]);
        if (!users.Any())
        {
            throw new Exception("User not found");
        }
        var user = users.First();
        var userActivityHistory = await _userActivityHistoryService.GetUserActivityHistory(userId);
        userActivityHistory.Sort((x, y) => y.ActivityDate.CompareTo(x.ActivityDate)); 
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

    