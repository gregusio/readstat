namespace Backend.DTO;

public class BookDTO
{
    public int Id { get; set; }
    public string? Title { get; set; } 
    public string? Author { get; set; }
    public string? AdditionalAuthors { get; set; } 
    public string? ISBN { get; set; } 
    public string? ISBN13 { get; set; } 
    public double AverageRating { get; set; } 
    public string? Publisher { get; set; } 
    public int? NumberOfPages { get; set; } 
    public int? YearPublished { get; set; } 
    public int? OriginalPublicationYear { get; set; } 
    public int? MyRating { get; set; }
    public required string ExclusiveShelf { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime? DateAdded { get; set; }
    public string? MyReview { get; set; }
    public int? ReadCount { get; set; }
}