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

    [HttpPut("user/{userId}/update")]
    public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserProfileDTO userProfileDto)
    {
        if (userProfileDto == null)
        {
            return BadRequest(new { Message = "Invalid user profile data" });
        }

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