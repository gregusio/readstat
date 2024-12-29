namespace Backend.Models;

public class Book
{
    public int Id { get; set; }
    public string? ISBN { get; set; }
    public string? ISBN13 { get; set; }
    public required string Title { get; set; }
    public required string[] Authors { get; set; }
    public string? Publisher { get; set; }
    public int Rating { get; set; }
    public double AverageRating { get; set; }
    public required string Shelf { get; set; }
}
