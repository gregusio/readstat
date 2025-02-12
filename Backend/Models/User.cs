using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    public List<UserBookRecord>? UserBookRecords { get; set; }
}
