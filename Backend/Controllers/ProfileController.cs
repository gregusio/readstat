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

    [HttpGet("user")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userProfile = await _profileService.GetUserProfile(userId);
        return Ok(userProfile);
    }

    [HttpPut("user")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDTO userProfileDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updatedUserProfile = await _profileService.UpdateUserProfile(userId, userProfileDto);
        return Ok(updatedUserProfile);
    }

    [HttpGet("user/activity-history")]
    public async Task<IActionResult> GetUserActivityHistory()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userProfile = await _profileService.GetUserProfile(userId);
        return Ok(userProfile.UserActivityHistory);
    }
}