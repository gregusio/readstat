using Backend.DTO;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class BookService(BooksRepository bookRepository, UserBookRecordsRepository userBookRecordRepository)
{
    private readonly BooksRepository _bookRepository = bookRepository;
    private readonly UserBookRecordsRepository _userBookRecordRepository = userBookRecordRepository;

    public async Task<IEnumerable<BookBasicInfoDTO>> GetUserBooksAsync(int userId)
    {
        var userBookRecords = await _userBookRecordRepository.GetAllForUserAsync(userId);

        var DTOBooks = new List<BookBasicInfoDTO>();

        foreach (var userBookRecord in userBookRecords)
        {
            var book = await _bookRepository.GetByIdAsync(userBookRecord.BookId);

            DTOBooks.Add(new BookBasicInfoDTO
            {
                Id = userBookRecord.Id,
                Title = userBookRecord.UserTitle ?? book!.Title,
                Author = userBookRecord.UserAuthor ?? book!.Author,
                ExclusiveShelf = userBookRecord.ExclusiveShelf
            });
        }

        return DTOBooks;
    }

    public async Task<BookDTO> GetBookDetailsAsync(int userId, int recordId)
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

        return new BookDTO
        {
            Id = userBookRecord.Id,
            Title = userBookRecord.UserTitle ?? book!.Title,
            Author = userBookRecord.UserAuthor ?? book!.Author,
            AdditionalAuthors = userBookRecord.UserAdditionalAuthors ?? book!.AdditionalAuthors,
            AverageRating = book!.AverageRating,
            NumberOfPages = userBookRecord.UserNumberOfPages ?? book!.NumberOfPages,
            YearPublished = userBookRecord.UserYearPublished ?? book!.YearPublished,
            OriginalPublicationYear = userBookRecord.UserOriginalPublicationYear ?? book!.OriginalPublicationYear,
            ISBN = userBookRecord.UserISBN ?? book!.ISBN,
            ISBN13 = userBookRecord.UserISBN13 ?? book!.ISBN13,
            Publisher = userBookRecord.UserPublisher ?? book!.Publisher,
            MyRating = userBookRecord.MyRating,
            ExclusiveShelf = userBookRecord.ExclusiveShelf,
            DateRead = userBookRecord.DateRead,
            DateAdded = userBookRecord.DateAdded,
            MyReview = userBookRecord.MyReview,
            ReadCount = userBookRecord.ReadCount
        };
    }

    public async Task<BookDTO> AddBookAsync(int userId, BookDTO book)
    {
        var newBook = new Book
        {
            Title = book.Title,
            Author = book.Author,
            AdditionalAuthors = book.AdditionalAuthors,
            AverageRating = book.AverageRating,
            NumberOfPages = book.NumberOfPages,
            YearPublished = book.YearPublished,
            OriginalPublicationYear = book.OriginalPublicationYear,
            ISBN = book.ISBN,
            ISBN13 = book.ISBN13,
            Publisher = book.Publisher
        };

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
            UserISBN = book.ISBN,
            UserISBN13 = book.ISBN13,
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

    public async Task<BookDTO> UpdateBookAsync(int userId, BookDTO book)
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
        userBookRecord.UserISBN = book.ISBN;
        userBookRecord.UserISBN13 = book.ISBN13;
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
}