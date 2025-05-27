namespace Backend.Models;

public class UserFriends
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FriendId { get; set; }

    public DateTime FriendshipDate { get; set; } = DateTime.UtcNow;
}