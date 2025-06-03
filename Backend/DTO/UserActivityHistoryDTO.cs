namespace Backend.DTO;

public class UserActivityHistoryDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? ActivityType { get; set; }
    public DateTime ActivityDate { get; set; }
    public string? Description { get; set; }
    public int Likes { get; set; } = 0;
    public bool IsLiked { get; set; } = false;
}