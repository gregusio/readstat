using Backend.DTO;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var response = _authService.Register(request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (
            request == null
            || string.IsNullOrEmpty(request.Email)
            || string.IsNullOrEmpty(request.Password)
        )
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var token = _authService.Authenticate(request.Email, request.Password);

        if (token == null)
        {
            return Unauthorized(new { Message = "Invalid username or password" });
        }

        return Ok(new LoginResponse { Token = token, Message = "Login successful" });
    }
}
