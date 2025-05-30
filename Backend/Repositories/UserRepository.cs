using Backend.Data;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository(IDbContextFactory<DataContext> contextFactory) : IUserRepository
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<User> AddUserAsync(User user)
    {
        await using var _context = _contextFactory.CreateDbContext();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<int> userIds)
    {
        if (userIds == null || !userIds.Any())
        {
            return Enumerable.Empty<User>();
        }

        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }

    public async Task UpdateUserAsync(int userId, UserProfileDTO userProfileDto)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception("User not found");
        var isUsernameTaken = await _context.Users.AnyAsync(u => u.Username == userProfileDto.Username && u.Id != userId);
        if (isUsernameTaken)
        {
            throw new Exception("Username is already taken");
        }
        user.Username = userProfileDto.Username ?? user.Username;
        // TODO: avatarUrl and bio should be added to the User entity

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Users
            .Where(u => u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        await using var _context = _contextFactory.CreateDbContext();
        return await _context.Users.ToListAsync();
    }
}