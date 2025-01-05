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

    public string? UserTitle { get; set; }

    public string? UserAuthor { get; set; }

    public string? UserAdditionalAuthors { get; set; }

    public string? UserISBN { get; set; }

    public string? UserISBN13 { get; set; }

    public string? UserPublisher { get; set; }

    public int? UserNumberOfPages { get; set; }

    public int? UserYearPublished { get; set; }

    public int? UserOriginalPublicationYear { get; set; }

    public int MyRating { get; set; }

    public required string ExclusiveShelf { get; set; }

    public DateTime? DateRead { get; set; }

    public DateTime? DateAdded { get; set; }

    public string? MyReview { get; set; }

    public int? ReadCount { get; set; }
}
