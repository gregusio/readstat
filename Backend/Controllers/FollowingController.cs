using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FollowingController(IUserFollowingService userFollowingService) : ControllerBase
{
    private readonly IUserFollowingService _userFollowingService = userFollowingService;

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserFollowing(int userId)
    {
        var following = await _userFollowingService.GetUserFollowingAsync(userId);
        return Ok(following);
    }

    [HttpPost("user/{userId}/add/{followingId}")]
    public async Task<IActionResult> AddFollowing(int userId, int followingId)
    {
        await _userFollowingService.AddFollowingAsync(userId, followingId);
        return NoContent();
    }

    [HttpDelete("user/{userId}/remove/{followingId}")]
    public async Task<IActionResult> RemoveFollowing(int userId, int followingId)
    {
        await _userFollowingService.RemoveFollowingAsync(userId, followingId);
        return NoContent();
    }
}