namespace Backend.DTO;

public class StatisticsReadingProgressDTO
{
    public Dictionary<string, int> BooksReadPerMonth { get; set; } = new();
    public Dictionary<string, int> PagesReadPerMonth { get; set; } = new();
    public string MostProductiveYear { get; set; } = string.Empty;
    public string MostProductiveMonth { get; set; } = string.Empty;
    public double AveragePagesPerBook { get; set; }
    public int TotalPagesRead { get; set; }
}