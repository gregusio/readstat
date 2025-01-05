using System.Security.Claims;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StatisticsController(IStatisticsService statisticsService) : ControllerBase
{
    private readonly IStatisticsService _statisticsService = statisticsService;

    [HttpGet("summary")]
    public async Task<IActionResult> GetOverallSummary()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsSummary = await _statisticsService.GetStatisticsSummary(userId);
        return Ok(statisticsSummary);
    }

    [HttpGet("read-books")]
    public async Task<IActionResult> GetReadBooksSummary()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsReadBooks = await _statisticsService.GetStatisticsReadBooks(userId);
        return Ok(statisticsReadBooks);
    }
}