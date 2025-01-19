using System.Text;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Backend.test.Services;

public class GoodreadsFileServiceTest
{
    private Mock<IBookRepository> _bookRepositoryMock;
    private Mock<IUserBookRecordRepository> _userBookRecordRepositoryMock;
    private readonly GoodreadsFileService _goodreadsFileService;

    public GoodreadsFileServiceTest()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _userBookRecordRepositoryMock = new Mock<IUserBookRecordRepository>();
        _goodreadsFileService = new GoodreadsFileService(_bookRepositoryMock.Object, _userBookRecordRepositoryMock.Object);
    }

    [Fact]
    public async Task ProcessFile_ShouldReturnBooksCount()
    {
        // Arrange
        var filePath = "../../../TestFiles/goodreads_library_export.csv";
        var fileMock = new Mock<IFormFile>();
        var content = File.ReadAllBytes(filePath);
        var ms = new MemoryStream(content);
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(filePath);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);
        _userBookRecordRepositoryMock.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<UserBookRecord>>())).Verifiable();
        _bookRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Book { ISBN = ISBN.Create("1234567890"), ISBN13 = ISBN.Create("1234567890123") });
        _bookRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Book>())).ReturnsAsync(1);

        // Act

        var booksCount = await _goodreadsFileService.ProcessFile(fileMock.Object, 1);

        // Assert

        Assert.Equal(10, booksCount);
    }

    [Fact]
    public async Task ProcessFile_ShouldSaveBooksToDb()
    {
        // Arrange
        var filePath = "../../../TestFiles/goodreads_library_export.csv";
        var fileMock = new Mock<IFormFile>();
        var content = File.ReadAllBytes(filePath);
        var ms = new MemoryStream(content);
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(filePath);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);
        _userBookRecordRepositoryMock.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<UserBookRecord>>())).Verifiable();
        _bookRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Book() { ISBN = ISBN.Create("1234567890"), ISBN13 = ISBN.Create("1234567890123") });
        _bookRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Book>())).ReturnsAsync(1);

        // Act

        await _goodreadsFileService.ProcessFile(fileMock.Object, 1);

        // Assert

        _userBookRecordRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<UserBookRecord>>()), Times.Once);
    }

    [Fact]
    public async Task ProcessFile_ShouldReturnZero_WhenFileIsEmpty()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = Encoding.UTF8.GetBytes("Title,Author,ISBN,My Rating,Average Rating,Publisher,Binding,Year Published,Original Publication Year,Date Read,Date Added,Bookshelves,Bookshelves with positions,Exclusive Shelf,My Review,Spoiler,Private Notes,Read Count,Recommended For,Recommended By,Owned,Cover\n");

        var ms = new MemoryStream(content);
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        // Act

        var booksCount = await _goodreadsFileService.ProcessFile(fileMock.Object, 1);

        // Assert

        Assert.Equal(0, booksCount);
    }
}