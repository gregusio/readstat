namespace Backend.Controllers;

using System.Security.Claims;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FeedController(IFeedService feedService) : ControllerBase
{
    private readonly IFeedService _feedService = feedService;

    [HttpGet("get")]
    public async Task<IActionResult> GetUserFeed()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var feed = await _feedService.GetUserFeedAsync(userId);
        return Ok(feed);
    }

    [HttpPost("like/{activityId}")]
    public async Task<IActionResult> LikeActivity(int activityId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _feedService.LikeActivityAsync(userId, activityId);
        return NoContent();
    }

    [HttpPost("unlike/{activityId}")]
    public async Task<IActionResult> UnlikeActivity(int activityId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _feedService.UnlikeActivityAsync(userId, activityId);
        return NoContent();
    }
}