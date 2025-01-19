namespace Backend.DTO;

public class LoginResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public required bool Success { get; set; }
    public required string Message { get; set; }
}
