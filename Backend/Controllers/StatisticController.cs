using System.Security.Claims;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StatisticController(IStatisticService statisticsService) : ControllerBase
{
    private readonly IStatisticService _statisticsService = statisticsService;

    [HttpGet("user/{userId}/summary")]
    public async Task<IActionResult> GetOverallSummary(int userId)
    {
        var statisticsSummary = await _statisticsService.GetStatisticsSummary(userId);
        return Ok(statisticsSummary);
    }

    [HttpGet("user/{userId}/read-books")]
    public async Task<IActionResult> GetReadBooksSummary(int userId)
    {
        var statisticsReadBooks = await _statisticsService.GetStatisticsReadBooks(userId);
        return Ok(statisticsReadBooks);
    }

    [HttpGet("user/{userId}/progress")]
    public async Task<IActionResult> GetReadingProgress(int userId)
    {
        var statisticsReadingProgress = await _statisticsService.GetStatisticsReadingProgress(userId);
        return Ok(statisticsReadingProgress);
    }

    [HttpGet("user/{userId}/monthly-read-books")]
    public async Task<IActionResult> GetMonthlyReadBooks(int userId)
    {
        var statisticsMonthlyReadBooks = await _statisticsService.GetStatisticsMonthlyReadBookCountPerYear(userId);
        return Ok(statisticsMonthlyReadBooks);
    }

    [HttpGet("user/{userId}/monthly-read-pages")]
    public async Task<IActionResult> GetMonthlyReadPages(int userId)
    {
        var statisticsMonthlyReadPages = await _statisticsService.GetStatisticsMonthlyReadPageCountPerYear(userId);
        return Ok(statisticsMonthlyReadPages);
    }

    [HttpGet("user/{userId}/monthly-added-books")]
    public async Task<IActionResult> GetMonthlyAddedBooks(int userId)
    {
        var statisticsMonthlyAddedBooks = await _statisticsService.GetStatisticsMonthlyAddedBookCountPerYear(userId);
        return Ok(statisticsMonthlyAddedBooks);
    }

    [HttpGet("user/{userId}/yearly-read-books")]
    public async Task<IActionResult> GetYearlyReadBooks(int userId)
    {
        var statisticsYearlyReadBooks = await _statisticsService.GetStatisticsYearlyReadBookCountPerYear(userId);
        return Ok(statisticsYearlyReadBooks);
    }

    [HttpGet("user/{userId}/yearly-read-pages")]
    public async Task<IActionResult> GetYearlyReadPages(int userId)
    {
        var statisticsYearlyReadPages = await _statisticsService.GetStatisticsYearlyReadPageCountPerYear(userId);
        return Ok(statisticsYearlyReadPages);
    }

    [HttpGet("user/{userId}/yearly-added-books")]
    public async Task<IActionResult> GetYearlyAddedBooks(int userId)
    {
        var statisticsYearlyAddedBooks = await _statisticsService.GetStatisticsYearlyAddedBookCountPerYear(userId);
        return Ok(statisticsYearlyAddedBooks);
    }

    [HttpGet("user/{userId}/most-read-authors")]
    public async Task<IActionResult> GetMostReadAuthors(int userId)
    {
        var statisticsMostReadAuthors = await _statisticsService.GetStatisticsMostReadAuthors(userId);
        return Ok(statisticsMostReadAuthors);
    }
}