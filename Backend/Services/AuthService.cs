using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class AuthService(IConfiguration configuration, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository) : IAuthService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user != null && user.PasswordHash == HashPassword(password))
        {
            var (accessToken, refreshToken) = await GenerateTokens(username);
            return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken, Success = true, Message = "Login successful" };
        }

        return new LoginResponse { Success = false, Message = "Invalid username or password" };
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (token == null || token.ExpiresAt < DateTime.UtcNow)
        {
            return new RefreshTokenResponse { Success = false, Message = "Invalid refresh token" };
        }

        var users = await _userRepository.GetByIdsAsync(new[] { token.UserId });
        var user = users.FirstOrDefault();
        if (user == null)
        {
            return new RefreshTokenResponse { Success = false, Message = "User not found" };
        }

        var (newAccessToken, newRefreshToken) = await GenerateTokens(user.Username);

        return new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            Success = true,
            Message = "Token refreshed"
        };
    }

    private async Task<(string accessToken, string refreshToken)> GenerateTokens(string username)
    {
        var accessToken = await GenerateJwtToken(username);
        var refreshToken = GenerateRefreshToken();

        SaveRefreshToken(username, refreshToken);

        return (accessToken, refreshToken);
    }

    private async Task<string> GenerateJwtToken(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier,  user!.Id.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!.PadRight(16, '0'))
        );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        RandomNumberGenerator.Fill(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async void SaveRefreshToken(string username, string refreshToken)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return;
        }

        var token = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepository.AddAsync(token);
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.GetByUsernameAsync(request.Username) != null)
        {
            return new RegisterResponse { Success = false, Message = "Username already taken" };
        }

        var passwordHash = HashPassword(request.Password);

        var user = new User
        {
            PasswordHash = passwordHash,
            Username = request.Username
        };
        await _userRepository.AddUserAsync(user);

        return new RegisterResponse { Success = true, Message = "Registration successful" };
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
