using Backend.DTO;

namespace Backend.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookBasicInfoDTO>> GetUserBooksAsync(int userId);
    Task<BookDTO> GetBookDetailsAsync(int userId, int recordId);
    Task<BookDTO> AddBookAsync(int userId, BookDTO book);
    Task<BookDTO> UpdateBookAsync(int userId, BookDTO book);
    Task DeleteBookAsync(int userId, int bookId);
}