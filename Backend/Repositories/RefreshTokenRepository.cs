using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Backend.Repositories;

public class RefreshTokenRepository(IDbContextFactory<DataContext> contextFactory)
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task AddAsync(RefreshToken token)
    {
        await using var _context = _contextFactory.CreateDbContext();
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        if (refreshToken == null)
        {
            return null;
        }
        return refreshToken;
    }

    public async Task DeletePreviousAsync(int userId)
    {
        await using var _context = _contextFactory.CreateDbContext();
        var tokens = _context.RefreshTokens.Where(t => t.UserId == userId);
        _context.RefreshTokens.RemoveRange(tokens);
        await _context.SaveChangesAsync();
    }
}