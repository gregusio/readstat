using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository(IDbContextFactory<DataContext> contextFactory)
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<User> AddUserAsync(User user)
    {
        await using var _context = _contextFactory.CreateDbContext();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByUsernameAsync(string email)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}