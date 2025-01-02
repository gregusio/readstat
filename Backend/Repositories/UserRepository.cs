using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository(IDbContextFactory<DataContext> contextFactory)
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public User AddUser(User user)
    {
        using var _context = _contextFactory.CreateDbContext();
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User? GetByUsername(string email)
    {
        using var _context = _contextFactory.CreateDbContext();
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public User? GetById(int id)
    {
        using var _context = _contextFactory.CreateDbContext();
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }
}