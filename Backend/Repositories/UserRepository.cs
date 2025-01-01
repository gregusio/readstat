using Backend.Data;
using Backend.Models;

namespace Backend.Repositories;

public class UserRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public User AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User? GetByUsername(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public User? GetById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }
}