using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<Book> Books { get; set; }
    public required DbSet<UserBookRecord> UserBookRecords { get; set; }
    public required DbSet<UserActivityHistory> UserActivityHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Book>()
            .Property(b => b.ISBN)
            .HasConversion(
                isbn => isbn!.ToString(),
                value => ISBN.Create(value)
            )
            .HasColumnName("ISBN")
            .IsRequired();

        modelBuilder.Entity<Book>()
            .Property(b => b.ISBN13)
            .HasConversion(
                isbn => isbn!.ToString(),
                value => ISBN.Create(value)
            )
            .HasColumnName("ISBN13")
            .IsRequired();

        modelBuilder.Entity<UserBookRecord>()
            .Property(ubr => ubr.UserISBN)
            .HasConversion(
                isbn => isbn!.ToString(),
                value => ISBN.Create(value)
            )
            .HasColumnName("UserISBN")
            .IsRequired();

        modelBuilder.Entity<UserBookRecord>()
            .Property(ubr => ubr.UserISBN13)
            .HasConversion(
                isbn => isbn!.ToString(),
                value => ISBN.Create(value)
            )
            .HasColumnName("UserISBN13")
            .IsRequired();
    }
}
