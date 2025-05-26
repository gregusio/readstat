namespace Backend.DTO;

public class UserProfileDTO
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public List<UserActivityHistoryDTO>? UserActivityHistory { get; set; }
}