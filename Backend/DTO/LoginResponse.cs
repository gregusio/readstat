namespace Backend.DTO;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required string Message { get; set; }
}
