using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class BooksRepository(IDbContextFactory<DataContext> contextFactory)
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<Book?> GetByIdAsync(int id)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> GetByIsbnAsync(string? isbn)
    {
        if (string.IsNullOrEmpty(isbn))
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

    public async Task AddAsync(Book book)
    {
        await using var _context = _contextFactory.CreateDbContext();

        if (await _context.Books.AnyAsync(b => b.ISBN == book.ISBN) && !string.IsNullOrEmpty(book.ISBN))
        {
            // throw new InvalidOperationException("Book with this ISBN already exists");
        }

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
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
