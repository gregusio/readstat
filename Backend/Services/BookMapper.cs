using Backend.DTO;
using Backend.Models;

namespace Backend.Services;

public class BookMapper
{
    public static Book Map(BookDetailsDTO bookDto)
    {
        return new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            AdditionalAuthors = bookDto.AdditionalAuthors,
            AverageRating = bookDto.AverageRating,
            NumberOfPages = bookDto.NumberOfPages,
            YearPublished = bookDto.YearPublished,
            OriginalPublicationYear = bookDto.OriginalPublicationYear,
            ISBN = ISBN.Create(bookDto.ISBN),
            ISBN13 = ISBN.Create(bookDto.ISBN13),
            Publisher = bookDto.Publisher,
        };
    }

    public static BookDetailsDTO Map(Book book, UserBookRecord userBookRecord)
    {
        return new BookDetailsDTO
        {
            Id = userBookRecord.Id,
            Title = userBookRecord.UserTitle ?? book.Title,
            Author = userBookRecord.UserAuthor ?? book.Author,
            AdditionalAuthors = userBookRecord.UserAdditionalAuthors ?? book.AdditionalAuthors,
            AverageRating = book.AverageRating,
            NumberOfPages = userBookRecord.UserNumberOfPages ?? book.NumberOfPages,
            YearPublished = userBookRecord.UserYearPublished ?? book.YearPublished,
            OriginalPublicationYear = userBookRecord.UserOriginalPublicationYear ?? book.OriginalPublicationYear,
            ISBN = userBookRecord.UserISBN?.Value ?? book.ISBN?.Value,
            ISBN13 = userBookRecord.UserISBN13?.Value ?? book.ISBN13?.Value,
            Publisher = userBookRecord.UserPublisher ?? book.Publisher,
            MyRating = userBookRecord.MyRating,
            ExclusiveShelf = userBookRecord.ExclusiveShelf,
            DateRead = userBookRecord.DateRead,
            DateAdded = userBookRecord.DateAdded,
            MyReview = userBookRecord.MyReview,
            ReadCount = userBookRecord.ReadCount
        };
    }
}