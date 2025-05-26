using Backend.DTO;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IUserRepository userRepository) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IUserRepository _userRepository = userRepository;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Username))
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var response = await _authService.RegisterAsync(request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (
            request == null
            || string.IsNullOrEmpty(request.Username)
            || string.IsNullOrEmpty(request.Password)
        )
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var response = await _authService.LoginAsync(request.Username, request.Password);

        if (response == null)
        {
            return Unauthorized(new { Message = "Invalid username or password" });
        }

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
    {
        if (model == null || string.IsNullOrEmpty(model.RefreshToken))
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var response = await _authService.RefreshTokenAsync(model.RefreshToken);

        if (response == null)
        {
            return BadRequest(new { Message = "Invalid refresh token" });
        }

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var username = User.Identity?.Name;
        if (username == null)
        {
            return Unauthorized();
        }

        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = user.Id,
            username = user.Username,
        });
    }
}
