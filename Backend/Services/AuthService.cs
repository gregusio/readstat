using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.DTO;
using Backend.Models;
using Backend.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class AuthService(IConfiguration configuration, UserRepository userRepository, RefreshTokenRepository refreshTokenRepository)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserRepository _userRepository = userRepository;
    private readonly RefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    public async Task<LoginResponse> Authenticate(string username, string password)
    {
        // TODO: Replace this with a real authentication logic
        if (username == "test" && password == "password")
        {
            var tokens = GenerateTokens(username);

            return new LoginResponse
            {
                AccessToken = tokens.accessToken,
                RefreshToken = tokens.refreshToken,
                Message = "Login successful"
            };
        }
        return new LoginResponse { AccessToken = "null", RefreshToken = "null", Message = "Invalid username or password" };
    }

    public async Task<RefreshTokenResponse?> RefreshToken(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByToken(refreshToken);
        if (token == null || token.ExpiresAt < DateTime.UtcNow)
        {
            return null;
        }

        var user = _userRepository.GetById(token.UserId);
        if (user == null)
        {
            return null;
        }

        await _refreshTokenRepository.Delete(token);

        var (newAccessToken, newRefreshToken) = GenerateTokens(user.Email);

        return new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    private (string accessToken, string refreshToken) GenerateTokens(string username)
    {
        var accessToken = GenerateJwtToken(username);
        var refreshToken = GenerateRefreshToken();

        SaveRefreshToken(username, refreshToken);

        return (accessToken, refreshToken);
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier, _userRepository.GetByUsername(username)!.Id.ToString())
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
        var user = _userRepository.GetByUsername(username);
        if (user == null)
        {
            return;
        }

        await _refreshTokenRepository.DeletePrevious(user.Id);

        var token = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepository.Add(token);
    }

    public RegisterResponse Register(RegisterRequest request)
    {
        if (_userRepository.GetByUsername(request.Email) != null)
        {
            return new RegisterResponse { Success = false, Message = "Username already taken" };
        }

        var passwordHash = HashPassword(request.Password);

        var user = new User
        {
            PasswordHash = passwordHash,
            Email = request.Email
        };
        _userRepository.AddUser(user);

        return new RegisterResponse { Success = true, Message = "Registration successful" };
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
