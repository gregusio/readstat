using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpPost("add/{followingId}")]
    public async Task<IActionResult> AddFollowing(int followingId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _userFollowingService.AddFollowingAsync(userId, followingId);
        return NoContent();
    }

    [HttpDelete("remove/{followingId}")]
    public async Task<IActionResult> RemoveFollowing(int followingId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _userFollowingService.RemoveFollowingAsync(userId, followingId);
        return NoContent();
    }
}