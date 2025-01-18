using System.Security.Cryptography;
using System.Text;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Backend.test.Services;

public class AuthServiceTest
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IConfiguration> _configMock;
    private Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private AuthService _authService;

    public AuthServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _configMock = new Mock<IConfiguration>();
        _authService = new AuthService(_configMock.Object, _userRepositoryMock.Object, _refreshTokenRepositoryMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        using var sha256 = SHA256.Create();

        var Sha256Hash = new Func<string, string>(input =>
        {
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        });

        var user = new User
        {
            Id = 1,
            Email = "test",
            PasswordHash = Sha256Hash("test")
        };

        _configMock.Setup(x => x["Jwt:Key"]).Returns("This is a test key and should be changed in production");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("test");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("test");

        _userRepositoryMock.Setup(x => x.GetByUsernameAsync("test")).ReturnsAsync(user);

        // Act
        var result = await _authService.LoginAsync("test", "test");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Login successful", result.Message);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidCredentials_ReturnsNull()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetByUsernameAsync("test")).ReturnsAsync((User?)null);

        // Act
        var result = await _authService.LoginAsync("test", "test");

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task RegisterAsync_WithValidCredentials_ReturnsRegisterResponse()
    {
        // Arrange
        using var sha256 = SHA256.Create();

        var Sha256Hash = new Func<string, string>(input =>
        {
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        });

        var user = new User
        {
            Id = 1,
            Email = "test",
            PasswordHash = Sha256Hash("test")
        };

        _configMock.Setup(x => x["Jwt:Key"]).Returns("This is a test key and should be changed in production");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("test");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("test");

        _userRepositoryMock.Setup(x => x.GetByUsernameAsync("test")).ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(x => x.AddUserAsync(It.IsAny<User>())).ReturnsAsync(user);

        // Act
        var result = await _authService.RegisterAsync(new DTO.RegisterRequest { Email = "test", Password = "test" });

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal("Registration successful", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_WithInvalidCredentials_ReturnsRegisterResponse()
    {
        // Arrange
        using var sha256 = SHA256.Create();

        var Sha256Hash = new Func<string, string>(input =>
        {
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        });

        var user = new User
        {
            Id = 1,
            Email = "test",
            PasswordHash = Sha256Hash("test")
        };

        _configMock.Setup(x => x["Jwt:Key"]).Returns("This is a test key and should be changed in production");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("test");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("test");

        _userRepositoryMock.Setup(x => x.GetByUsernameAsync("test")).ReturnsAsync(user);

        // Act
        var result = await _authService.RegisterAsync(new DTO.RegisterRequest { Email = "test", Password = "test" });

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal("Username already taken", result.Message);
    }

    [Fact]
    public async Task RefreshTokenAsync_WithValidToken_ReturnsLoginResponse()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Id = 1,
            Token = "test",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        var user = new User
        {
            Id = 1,
            Email = "test",
            PasswordHash = "test"
        };

        _configMock.Setup(x => x["Jwt:Key"]).Returns("This is a test key and should be changed in production");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("test");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("test");

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync("test")).ReturnsAsync(refreshToken);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.GetByUsernameAsync("test")).ReturnsAsync(user);

        // Act
        var result = await _authService.RefreshTokenAsync("test");

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
    }

}