using Backend.DTO;

namespace Backend.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(string username, string password);
    Task<RefreshTokenResponse?> RefreshTokenAsync(string refreshToken);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
}