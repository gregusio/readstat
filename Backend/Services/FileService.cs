using System.Globalization;
using Backend.Models;
using Backend.Repositories;
using CsvHelper;
using CsvHelper.Configuration;

namespace Backend.Services;

public class FileService(BooksRepository booksRepository, UserBookRecordsRepository userBookRecordsRepository)
{
    private readonly BooksRepository _booksRepository = booksRepository;
    private readonly UserBookRecordsRepository _userBookRecordsRepository = userBookRecordsRepository;

    public async Task<int> ProcessCsvFile(IFormFile file, int userId)
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
                MyRating = csvReader.GetField<int?>("My Rating"),
                ExclusiveShelf = csvReader.GetField("Exclusive Shelf")!,
                DateRead = DateTime.TryParse(csvReader.GetField("Date Read"), out var dateRead) ? dateRead : null,
                DateAdded = DateTime.TryParse(csvReader.GetField("Date Added"), out var dateAdded) ? dateAdded : null,
                MyReview = csvReader.GetField("My Review"),
                ReadCount = csvReader.GetField<int>("Read Count"),
            };

            bookRecords.Add(userBookRecord);
        }

        await _userBookRecordsRepository.AddRangeAsync(bookRecords);

        return bookRecords.Count;
    }

    private async Task<Book> FindOrCreateBook(CsvReader csvReader)
    {
        var isbn = NormalizeIsbn(csvReader.GetField("ISBN")!);


        var book = await _booksRepository.GetByIsbnAsync(isbn);

        if (book == null)
        {
            book = new Book
            {
                Title = csvReader.GetField("Title"),
                Author = csvReader.GetField("Author"),
                AdditionalAuthors = csvReader.GetField("Additional Authors"),
                ISBN = isbn,
                ISBN13 = NormalizeIsbn(csvReader.GetField("ISBN13")!),
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