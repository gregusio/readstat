using System.Security.Claims;
using Backend.DTO;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController(IProfileService profileService) : ControllerBase
{
    private readonly IProfileService _profileService = profileService;

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserProfile(int userId)
    {
        var userProfile = await _profileService.GetUserProfile(userId);
        if (userProfile == null)
        {
            return NotFound(new { Message = "User profile not found" });
        }
        return Ok(userProfile);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDTO userProfileDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (userProfileDto == null)
        {
            return BadRequest(new { Message = "Invalid user profile data" });
        }

        Console.WriteLine($"Updating profile for user ID: {userId}");
        Console.WriteLine($"Profile Data: Username={userProfileDto.Username}, AvatarUrl={userProfileDto.AvatarUrl}, Bio={userProfileDto.Bio}");
        var updatedProfile = await _profileService.UpdateUserProfile(userId, userProfileDto);
        if (updatedProfile == null)
        {
            return NotFound(new { Message = "User profile not found" });
        }
        return Ok(updatedProfile);
    }

    [HttpGet("user/{userId}/activity-history")]
    public async Task<IActionResult> GetUserActivityHistory(int userId)
    {
        var userProfile = await _profileService.GetUserProfile(userId);
        return Ok(userProfile.UserActivityHistory);
    }
}