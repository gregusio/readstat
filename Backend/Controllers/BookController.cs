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

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserBooks(int userId)
    {
        var books = await _bookService.GetUserBooksAsync(userId);
        return Ok(books);
    }

    [HttpGet("user/{userId}/book/{bookId}")]
    public async Task<IActionResult> GetBookDetails(int userId, int bookId)
    {
        var book = await _bookService.GetBookDetailsAsync(userId, bookId);
        return Ok(book);
    }

    [HttpPost("user/{userId}/book/add")]
    public async Task<IActionResult> AddBook(int userId, [FromBody] BookDetailsDTO book)
    {
        await _bookService.AddBookAsync(userId, book);
        return Ok(new Response { Message = "Book added successfully", Success = true });
    }

    [HttpPatch("user/{userId}/book/update")]
    public async Task<IActionResult> UpdateBook(int userId, [FromBody] BookDetailsDTO book)
    {
        await _bookService.UpdateBookAsync(userId, book);
        return Ok(new Response { Message = "Book updated successfully", Success = true });
    }

    [HttpDelete("user/{userId}/book/delete/{bookId}")]
    public async Task<IActionResult> DeleteBook(int userId, int bookId)
    {
        await _bookService.DeleteBookAsync(userId, bookId);
        return Ok(new Response { Message = "Book deleted successfully", Success = true });
    }

    [HttpDelete("user/{userId}/book/delete/all")]
    public async Task<IActionResult> DeleteAllBooks(int userId)
    {
        await _bookService.DeleteAllBooksAsync(userId);
        return Ok(new Response { Message = "All books deleted successfully", Success = true });
    }
}