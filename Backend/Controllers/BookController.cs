using System.Security.Claims;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookController(BookService bookService) : ControllerBase
{
    private readonly BookService _bookService = bookService;

    [HttpGet("user-books")]
    public async Task<IActionResult> GetUserBooks()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var books = await _bookService.GetUserBooksAsync(userId);
        return Ok(books);
    }
}