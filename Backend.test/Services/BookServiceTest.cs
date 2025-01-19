using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Moq;

namespace Backend.test.Services;

public class BookServiceTest
{
    private Mock<IBookRepository> _bookRepositoryMock;
    private Mock<IUserBookRecordRepository> _userBookRecordRepositoryMock;
    private BookService _bookService;

    public BookServiceTest()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _userBookRecordRepositoryMock = new Mock<IUserBookRecordRepository>();
        _bookService = new BookService(_bookRepositoryMock.Object, _userBookRecordRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUserBooksAsync_ShouldReturnBooks()
    {
        // Arrange
        var userBookRecords = new List<UserBookRecord>
        {
            new UserBookRecord
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                UserTitle = "Title",
                UserAuthor = "Author",
                UserAdditionalAuthors = "Additional Authors",
                UserISBN = ISBN.Create("1234567890"),
                UserISBN13 = ISBN.Create("1234567890123"),
                UserPublisher = "Publisher",
                UserNumberOfPages = 100,
                UserYearPublished = 2021,
                UserOriginalPublicationYear = 2020,
                MyRating = 5,
                ExclusiveShelf = "Read",
                DateRead = DateTime.Now,
                DateAdded = DateTime.Now,
                MyReview = "Great book!",
                ReadCount = 1
            }
        };

        var books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Title",
                Author = "Author",
                AdditionalAuthors = "Additional Authors",
                ISBN = ISBN.Create("1234567890"),
                ISBN13 = ISBN.Create("1234567890123"),
                AverageRating = 4.5,
                Publisher = "Publisher",
                NumberOfPages = 100,
                YearPublished = 2021,
                OriginalPublicationYear = 2020
            }
        };

        _userBookRecordRepositoryMock.Setup(x => x.GetAllForUserAsync(1)).ReturnsAsync(userBookRecords);
        _bookRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(books.First());

        // Act
        var result = await _bookService.GetUserBooksAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result.First().Id);
        Assert.Equal("Title", result.First().Title);
        Assert.Equal("Author", result.First().Author);
        Assert.Equal("Read", result.First().ExclusiveShelf);
    }

    [Fact]
    public async Task GetBookDetailsAsync_ShouldReturnBookDetails()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "test",
            PasswordHash = "test"
        };

        var userBookRecord = new UserBookRecord
        {
            Id = 1,
            UserId = 1,
            BookId = 1,
            UserTitle = "Title",
            UserAuthor = "Author",
            UserAdditionalAuthors = "Additional Authors",
            UserISBN = ISBN.Create("1234567890"),
            UserISBN13 = ISBN.Create("1234567890123"),
            UserPublisher = "Publisher",
            UserNumberOfPages = 100,
            UserYearPublished = 2021,
            UserOriginalPublicationYear = 2020,
            MyRating = 5,
            ExclusiveShelf = "Read",
            DateRead = DateTime.Now,
            DateAdded = DateTime.Now,
            MyReview = "Great book!",
            ReadCount = 1
        };

        var book = new Book
        {
            Id = 1,
            Title = "Title",
            Author = "Author",
            AdditionalAuthors = "Additional Authors",
            ISBN = ISBN.Create("1234567890"),
            ISBN13 = ISBN.Create("1234567890123"),
            AverageRating = 4.5,
            Publisher = "Publisher",
            NumberOfPages = 100,
            YearPublished = 2021,
            OriginalPublicationYear = 2020
        };

        _userBookRecordRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(userBookRecord);
        _bookRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(book);
        _userBookRecordRepositoryMock.Setup(x => x.GetByUserIdAndBookIdAsync(1, 1)).ReturnsAsync(userBookRecord);

        // Act
        var result = await _bookService.GetBookDetailsAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Title", result.Title);
        Assert.Equal("Author", result.Author);

    }

}


