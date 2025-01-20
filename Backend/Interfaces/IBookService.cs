using Backend.DTO;

namespace Backend.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookBasicInfoDTO>> GetUserBooksAsync(int userId);
    Task<BookDetailsDTO> GetBookDetailsAsync(int userId, int recordId);
    Task<BookDetailsDTO> AddBookAsync(int userId, BookDetailsDTO book);
    Task<BookDetailsDTO> UpdateBookAsync(int userId, BookDetailsDTO book);
    Task DeleteBookAsync(int userId, int bookId);
    Task DeleteAllBooksAsync(int userId);
}