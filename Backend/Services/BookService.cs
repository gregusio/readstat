using Backend.DTO;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class BookService(BooksRepository bookRepository, UserBookRecordsRepository userBookRecordRepository)
{
    private readonly BooksRepository _bookRepository = bookRepository;
    private readonly UserBookRecordsRepository _userBookRecordRepository = userBookRecordRepository;

    public async Task<IEnumerable<BookDTO>> GetUserBooksAsync(int userId)
    {
        var userBooks = await _userBookRecordRepository.GetAllForUserAsync(userId);

        var bookDetails = await _bookRepository.GetBooksByIdsAsync(userBooks.Select(ub => ub.BookId));

        var bookDtos = bookDetails.Select(book => new BookDTO
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            AdditionalAuthors = book.AdditionalAuthors,
            ISBN = book.ISBN,
            ISBN13 = book.ISBN13,
            AverageRating = book.AverageRating,
            Publisher = book.Publisher,
            NumberOfPages = book.NumberOfPages,
            YearPublished = book.YearPublished,
            OriginalPublicationYear = book.OriginalPublicationYear,
            MyRating = userBooks.FirstOrDefault(ub => ub.BookId == book.Id)?.MyRating,
            ExclusiveShelf = userBooks.FirstOrDefault(ub => ub.BookId == book.Id)?.ExclusiveShelf!,
            DateRead = userBooks.FirstOrDefault(ub => ub.BookId == book.Id)?.DateRead,
            DateAdded = userBooks.FirstOrDefault(ub => ub.BookId == book.Id)?.DateAdded,
            MyReview = userBooks.FirstOrDefault(ub => ub.BookId == book.Id)?.MyReview,
            ReadCount = userBooks.FirstOrDefault(ub => ub.BookId == book.Id)?.ReadCount
        });

        return bookDtos;
    }



}