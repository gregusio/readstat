namespace Backend.Models;

public class UserActivityHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? ActivityType { get; set; }
    public DateTime ActivityDate { get; set; }
    public string? Description { get; set; }
}