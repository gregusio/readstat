using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class UserBookRecord
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [ForeignKey("BookId")]
    public int BookId { get; set; }

    public required Book Book { get; set; }

    public int? MyRating { get; set; }

    public required string ExclusiveShelf { get; set; }

    public DateTime? DateRead { get; set; }

    public DateTime? DateAdded { get; set; }

    public string? MyReview { get; set; }

    public int ReadCount { get; set; }
}
