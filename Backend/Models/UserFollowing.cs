namespace Backend.Models;

public class UserFollowing
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FollowingId { get; set; }

    public DateTime FollowingDate { get; set; } = DateTime.UtcNow;
}