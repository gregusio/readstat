using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.DTO;
using Backend.Models;
using Backend.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class AuthService(IConfiguration configuration, UserRepository userRepository)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserRepository _userRepository = userRepository;

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
        return null;
    }

    private (string accessToken, string refreshToken) GenerateTokens(string username)
    {
        var accessToken = GenerateJwtToken(username);
        var refreshToken = GenerateRefreshToken();
        // TODO Save refresh token to database
        // SaveRefreshToken(user, refreshToken);

        return (accessToken, refreshToken);
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "User"),
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

    private void SaveRefreshToken(User user, string refreshToken)
    {
        // TODO Save refresh token to database
        // _refreshTokenRepository.Save(new RefreshToken
        // {
        //     UserId = user.Id,
        //     Token = refreshToken,
        //     ExpiryDate = DateTime.Now.AddDays(30) 
        // });
    }

    public RegisterResponse Register(RegisterRequest request)
    {
        if (_userRepository.GetUserByUsername(request.Email) != null)
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
