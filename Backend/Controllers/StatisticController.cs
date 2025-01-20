using System.Security.Claims;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StatisticController(IStatisticService statisticsService) : ControllerBase
{
    private readonly IStatisticService _statisticsService = statisticsService;

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

    [HttpGet("progress")]
    public async Task<IActionResult> GetReadingProgress()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsReadingProgress = await _statisticsService.GetStatisticsReadingProgress(userId);
        return Ok(statisticsReadingProgress);
    }

    [HttpGet("monthly-read-books")]
    public async Task<IActionResult> GetMonthlyReadBooks()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsMonthlyReadBooks = await _statisticsService.GetStatisticsMonthlyReadBookCountPerYear(userId);
        return Ok(statisticsMonthlyReadBooks);
    }

    [HttpGet("monthly-read-pages")]
    public async Task<IActionResult> GetMonthlyReadPages()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsMonthlyReadPages = await _statisticsService.GetStatisticsMonthlyReadPageCountPerYear(userId);
        return Ok(statisticsMonthlyReadPages);
    }

    [HttpGet("monthly-added-books")]
    public async Task<IActionResult> GetMonthlyAddedBooks()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsMonthlyAddedBooks = await _statisticsService.GetStatisticsMonthlyAddedBookCountPerYear(userId);
        return Ok(statisticsMonthlyAddedBooks);
    }

    [HttpGet("yearly-read-books")]
    public async Task<IActionResult> GetYearlyReadBooks()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsYearlyReadBooks = await _statisticsService.GetStatisticsYearlyReadBookCountPerYear(userId);
        return Ok(statisticsYearlyReadBooks);
    }

    [HttpGet("yearly-read-pages")]
    public async Task<IActionResult> GetYearlyReadPages()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsYearlyReadPages = await _statisticsService.GetStatisticsYearlyReadPageCountPerYear(userId);
        return Ok(statisticsYearlyReadPages);
    }

    [HttpGet("yearly-added-books")]
    public async Task<IActionResult> GetYearlyAddedBooks()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsYearlyAddedBooks = await _statisticsService.GetStatisticsYearlyAddedBookCountPerYear(userId);
        return Ok(statisticsYearlyAddedBooks);
    }

    [HttpGet("most-read-authors")]
    public async Task<IActionResult> GetMostReadAuthors()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var statisticsMostReadAuthors = await _statisticsService.GetStatisticsMostReadAuthors(userId);
        return Ok(statisticsMostReadAuthors);
    }
}