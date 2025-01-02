using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Book
{
    [Key]
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
}
