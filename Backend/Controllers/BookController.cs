using System.Security.Claims;
using Backend.DTO;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService;

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
    public async Task<IActionResult> AddBook([FromBody] BookDetailsDTO book)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        await _bookService.AddBookAsync(userId, book);
        return Ok(new Response { Message = "Book added successfully", Success = true });
    }

    [HttpPatch("update-book")]
    public async Task<IActionResult> UpdateBook([FromBody] BookDetailsDTO book)
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

    [HttpDelete("delete-all-books")]
    public async Task<IActionResult> DeleteAllBooks()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        await _bookService.DeleteAllBooksAsync(userId);
        return Ok(new Response { Message = "All books deleted successfully", Success = true });
    }
}