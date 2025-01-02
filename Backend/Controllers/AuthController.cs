using Backend.DTO;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService, UserRepository userRepository, RefreshTokenRepository refreshTokenRepository) : ControllerBase
{
    private readonly AuthService _authService = authService;
    private readonly UserRepository _userRepository = userRepository;
    private readonly RefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var response = await _authService.Register(request);

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
            || string.IsNullOrEmpty(request.Email)
            || string.IsNullOrEmpty(request.Password)
        )
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var response = await _authService.Authenticate(request.Email, request.Password);

        if (response == null)
        {
            return Unauthorized(new { Message = "Invalid username or password" });
        }

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
    {
        if (model == null || string.IsNullOrEmpty(model.RefreshToken))
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var response = _authService.RefreshToken(model.RefreshToken);

        if (response == null)
        {
            return BadRequest(new { Message = "Invalid refresh token" });
        }

        return Ok(response);
    }

}
