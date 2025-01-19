using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class BookRepository(IDbContextFactory<DataContext> contextFactory) : IBookRepository
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<Book?> GetByIdAsync(int id)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> GetByIsbnAsync(ISBN? isbn)
    {
        if (string.IsNullOrEmpty(isbn?.Value))
        {
            return null;
        }

        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Books.ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByIdsAsync(IEnumerable<int> ids)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Books.Where(b => ids.Contains(b.Id)).ToListAsync();
    }

    public async Task<int> AddAsync(Book book)
    {
        await using var _context = _contextFactory.CreateDbContext();

        // TODO: Uncomment this code after implementing ISBN uniqueness check
        // if (await _context.Books.AnyAsync(b => b.ISBN == book.ISBN) && !string.IsNullOrEmpty(book.ISBN))
        // {
        //      throw new InvalidOperationException("Book with this ISBN already exists");
        // }

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book.Id;
    }

    public async Task UpdateAsync(Book book)
    {
        await using var _context = _contextFactory.CreateDbContext();

        if (!await _context.Books.AnyAsync(b => b.Id == book.Id))
        {
            throw new InvalidOperationException("Book with this ID does not exist");
        }

        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }
}
