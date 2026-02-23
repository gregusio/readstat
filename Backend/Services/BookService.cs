using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class BookService(IBookRepository bookRepository, IUserBookRecordRepository userBookRecordRepository) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IUserBookRecordRepository _userBookRecordRepository = userBookRecordRepository;

    public async Task<IEnumerable<BookBasicInfoDTO>> GetUserBooksAsync(int userId)
    {
        var userBookRecords = await _userBookRecordRepository.GetAllForUserAsync(userId);
    
        var bookIds = userBookRecords.Select(ubr => ubr.BookId).Distinct();
        var books = (await _bookRepository.GetBooksByIdsAsync(bookIds))
            .ToDictionary(b => b.Id);
    
        return userBookRecords.Select(ubr => new BookBasicInfoDTO
        {
            Id = ubr.Id,
            Title = ubr.UserTitle ?? books[ubr.BookId].Title,
            Author = ubr.UserAuthor ?? books[ubr.BookId].Author,
            ExclusiveShelf = ubr.ExclusiveShelf
        });
    }


    public async Task<BookDetailsDTO> GetBookDetailsAsync(int userId, int recordId)
    {
        var userBookRecord = await _userBookRecordRepository.GetByUserIdAndBookIdAsync(userId, recordId);

        if (userBookRecord == null)
        {
            throw new InvalidOperationException("User does not have this book");
        }

        var bookId = userBookRecord.BookId;
        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book == null)
        {
            throw new InvalidOperationException("Book not found");
        }

        return BookMapper.Map(book, userBookRecord);
    }

    public async Task<BookDetailsDTO> AddBookAsync(int userId, BookDetailsDTO book)
    {
        var newBook = BookMapper.Map(book);

        var addedBookId = await _bookRepository.AddAsync(newBook);

        var userBookRecord = new UserBookRecord
        {
            UserId = userId,
            BookId = addedBookId,
            UserTitle = book.Title,
            UserAuthor = book.Author,
            UserAdditionalAuthors = book.AdditionalAuthors,
            UserNumberOfPages = book.NumberOfPages,
            UserYearPublished = book.YearPublished,
            UserOriginalPublicationYear = book.OriginalPublicationYear,
            UserISBN = ISBN.Create(book.Isbn),
            UserISBN13 = ISBN.Create(book.Isbn13),
            UserPublisher = book.Publisher,
            MyRating = book.MyRating,
            ExclusiveShelf = book.ExclusiveShelf,
            DateRead = book.DateRead,
            DateAdded = book.DateAdded,
            MyReview = book.MyReview,
            ReadCount = book.ReadCount
        };

        await _userBookRecordRepository.AddAsync(userBookRecord);

        return book;
    }

    public async Task<BookDetailsDTO> UpdateBookAsync(int userId, BookDetailsDTO book)
    {
        var userBookRecord = await _userBookRecordRepository.GetByUserIdAndBookIdAsync(userId, book.Id);

        if (userBookRecord == null)
        {
            throw new InvalidOperationException("User does not have this book");
        }

        userBookRecord.UserTitle = book.Title;
        userBookRecord.UserAuthor = book.Author;
        userBookRecord.UserAdditionalAuthors = book.AdditionalAuthors;
        userBookRecord.UserNumberOfPages = book.NumberOfPages;
        userBookRecord.UserYearPublished = book.YearPublished;
        userBookRecord.UserOriginalPublicationYear = book.OriginalPublicationYear;
        userBookRecord.UserISBN = ISBN.Create(book.Isbn);
        userBookRecord.UserISBN13 = ISBN.Create(book.Isbn13);
        userBookRecord.UserPublisher = book.Publisher;
        userBookRecord.MyRating = book.MyRating;
        userBookRecord.ExclusiveShelf = book.ExclusiveShelf;
        userBookRecord.DateRead = book.DateRead;
        userBookRecord.DateAdded = book.DateAdded;
        userBookRecord.MyReview = book.MyReview;
        userBookRecord.ReadCount = book.ReadCount;

        await _userBookRecordRepository.UpdateAsync(userBookRecord);

        return book;
    }

    public async Task DeleteBookAsync(int userId, int bookId)
    {
        var userBookRecord = await _userBookRecordRepository.GetByUserIdAndBookIdAsync(userId, bookId);

        await _userBookRecordRepository.DeleteAsync(userBookRecord!.Id);
    }

    public async Task DeleteAllBooksAsync(int userId)
    {
        var userBookRecords = await _userBookRecordRepository.GetAllForUserAsync(userId);

        foreach (var userBookRecord in userBookRecords)
        {
            await _userBookRecordRepository.DeleteAsync(userBookRecord.Id);
        }
    }
}