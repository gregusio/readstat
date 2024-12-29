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

    public string? Authenticate(string username, string password)
    {
        // TODO: Replace this with a real authentication logic
        if (username == "test" && password == "password")
        {
            return GenerateJwtToken(username);
        }
        return null;
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
