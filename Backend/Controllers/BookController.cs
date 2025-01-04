using System.Security.Claims;
using Backend.DTO;
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

    [HttpGet("book-details/{id}")]
    public async Task<IActionResult> GetBookDetails(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var book = await _bookService.GetBookDetailsAsync(userId, id);
        return Ok(book);
    }

    [HttpPost("add-book")]
    public async Task<IActionResult> AddBook([FromBody] BookDTO book)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        await _bookService.AddBookAsync(userId, book);
        return Ok(new Response { Message = "Book added successfully", Success = true });
    }

    [HttpPut("update-book")]
    public async Task<IActionResult> UpdateBook([FromBody] BookDTO book)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        await _bookService.UpdateBookAsync(userId, book);
        return Ok(new Response { Message = "Book updated successfully", Success = true });
    }

    [HttpDelete("delete-book/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        await _bookService.DeleteBookAsync(userId, id);
        return Ok(new Response { Message = "Book deleted successfully", Success = true });
    }
}