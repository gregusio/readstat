using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

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
}