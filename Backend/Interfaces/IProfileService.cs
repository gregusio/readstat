using Backend.DTO;

namespace Backend.Interfaces;

public interface IProfileService
{
    Task<UserProfileDTO> GetUserProfile(int userId);
    Task<UserProfileDTO> UpdateUserProfile(int userId, UserProfileDTO userProfileDto);
}