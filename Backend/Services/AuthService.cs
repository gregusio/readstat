using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services;

public class AuthService(IConfiguration configuration, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository) : IAuthService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var user = await userRepository.GetByUsernameAsync(username);

        if (user != null && VerifyPassword(user, password))
        {
            var (accessToken, refreshToken) = await GenerateTokens(user);
            return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken, Success = true, Message = "Login successful" };
        }

        return new LoginResponse { Success = false, Message = "Invalid username or password" };
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var token = await refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (token == null || token.ExpiresAt < DateTime.UtcNow)
        {
            return new RefreshTokenResponse { Success = false, Message = "Invalid refresh token" };
        }

        var users = await userRepository.GetByIdsAsync(new[] { token.UserId });
        var user = users.FirstOrDefault();
        if (user == null)
        {
            return new RefreshTokenResponse { Success = false, Message = "User not found" };
        }

        var (newAccessToken, newRefreshToken) = await GenerateTokens(user);

        return new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            Success = true,
            Message = "Token refreshed"
        };
    }

    private async Task<(string accessToken, string refreshToken)> GenerateTokens(User user)
    {
        var accessToken = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        await SaveRefreshToken(user, refreshToken);

        return (accessToken, refreshToken);
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier,  user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!.PadRight(16, '0'))
        );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
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

    private async Task SaveRefreshToken(User user, string refreshToken)
    {
        var token = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await refreshTokenRepository.AddAsync(token);
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.GetByUsernameAsync(request.Username) != null)
        {
            return new RegisterResponse { Success = false, Message = "Username already taken" };
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = string.Empty
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await userRepository.AddUserAsync(user);

        return new RegisterResponse { Success = true, Message = "Registration successful" };
    }

    private bool VerifyPassword(User user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result != PasswordVerificationResult.Failed;
    }
}
