using System.Globalization;
using Backend.Interfaces;
using Backend.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace Backend.Services;

public class GoodreadsFileService(IBookRepository booksRepository, IUserBookRecordRepository userBookRecordsRepository) : IFileService
{
    private readonly IBookRepository _booksRepository = booksRepository;
    private readonly IUserBookRecordRepository _userBookRecordsRepository = userBookRecordsRepository;

    public async Task<int> ProcessFile(IFormFile file, int userId)
    {
        var bookRecords = await ParseCsvFile(file, userId);

        await _userBookRecordsRepository.AddRangeAsync(bookRecords);

        return bookRecords.Count();
    }

    private async Task<IEnumerable<UserBookRecord>> ParseCsvFile(IFormFile file, int userId)
    {
        var conf = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };
        using var stream = new StreamReader(file.OpenReadStream());
        using var csvReader = new CsvReader(stream, conf);

        csvReader.Read();
        csvReader.ReadHeader();

        var bookRecords = new List<UserBookRecord>();

        while (csvReader.Read())
        {
            var book = await FindOrCreateBook(csvReader);

            var userBookRecord = new UserBookRecord
            {
                UserId = userId,
                BookId = book.Id,
                UserISBN = ISBN.Create(NormalizeIsbn(csvReader.GetField("ISBN")!)),
                UserISBN13 = ISBN.Create(NormalizeIsbn(csvReader.GetField("ISBN13")!)),
                MyRating = csvReader.GetField<int>("My Rating"),
                ExclusiveShelf = csvReader.GetField("Exclusive Shelf")!,
                DateRead = DateTime.TryParse(csvReader.GetField("Date Read"), out var dateRead) ? dateRead : null,
                DateAdded = DateTime.TryParse(csvReader.GetField("Date Added"), out var dateAdded) ? dateAdded : null,
                MyReview = csvReader.GetField("My Review"),
                ReadCount = csvReader.GetField<int>("Read Count"),
            };

            bookRecords.Add(userBookRecord);
        }

        return bookRecords;
    }

    private async Task<Book> FindOrCreateBook(CsvReader csvReader)
    {
        var book = await _booksRepository.GetByIsbnAsync(ISBN.Create(NormalizeIsbn(csvReader.GetField("ISBN")!)));

        if (book == null)
        {
            book = new Book
            {
                Title = csvReader.GetField("Title"),
                Author = csvReader.GetField("Author"),
                AdditionalAuthors = csvReader.GetField("Additional Authors"),
                ISBN = ISBN.Create(NormalizeIsbn(csvReader.GetField("ISBN")!)),
                ISBN13 = ISBN.Create(NormalizeIsbn(csvReader.GetField("ISBN13")!)),
                AverageRating = csvReader.GetField<double>("Average Rating"),
                Publisher = csvReader.GetField("Publisher"),
                NumberOfPages = csvReader.GetField<int?>("Number of Pages"),
                YearPublished = csvReader.GetField<int?>("Year Published"),
                OriginalPublicationYear = csvReader.GetField<int?>("Original Publication Year"),
            };

            await _booksRepository.AddAsync(book);
        }

        return book;
    }

    private string NormalizeIsbn(string isbn)
    {
        return isbn.Replace("=", "").Replace("\"", "");
    }
}