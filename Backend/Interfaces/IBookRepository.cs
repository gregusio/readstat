using Backend.Models;

namespace Backend.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByIsbnAsync(ISBN? isbn);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> GetBooksByIdsAsync(IEnumerable<int> ids);
    Task<int> AddAsync(Book book);
    Task UpdateAsync(Book book);
}