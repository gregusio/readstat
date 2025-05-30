namespace Backend.Controllers;

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
    public async Task<IActionResult> GetUserFeed(int userId)
    {
        var feed = await _feedService.GetUserFeedAsync(userId);
        return Ok(feed);
    }
}