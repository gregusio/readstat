namespace Backend.DTO;

public class StatisticsReadBooksDTO
{
    public double AverageRating { get; set; }
    public List<BookBasicInfoDTO>? BooksWithOneStarRating { get; set; }
    public List<BookBasicInfoDTO>? BooksWithTwoStarRating { get; set; }
    public List<BookBasicInfoDTO>? BooksWithThreeStarRating { get; set; }
    public List<BookBasicInfoDTO>? BooksWithFourStarRating { get; set; }
    public List<BookBasicInfoDTO>? BooksWithFiveStarRating { get; set; }
    public Dictionary<string, int>? MostFivePopularAuthorsWithBooksCount { get; set; }

}