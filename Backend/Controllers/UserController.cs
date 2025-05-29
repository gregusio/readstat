using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("search/{searchTerm}")]
    public async Task<IActionResult> SearchUsers(string searchTerm)
    {
        var users = await _userService.SearchUsersAsync(searchTerm);
        return Ok(users);
    }

    [HttpGet("all/except/{userId}")]
    public async Task<IActionResult> GetAllUsers(int userId)
    {
        var users = await _userService.GetAllUsersAsync(userId);
        return Ok(users);
    }

    [HttpGet("{userId}/username")]
    public async Task<IActionResult> GetUsernameById(int userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(new { Username = user.Username });
    }
}