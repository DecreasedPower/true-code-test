using Microsoft.EntityFrameworkCore;
using UserService.Db.Models;

namespace UserService.Db;

public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : DbContext(options)
{
  public DbSet<DbUser> Users { get; set; }
  public DbSet<DbRefreshToken> RefreshTokens { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<DbRefreshToken>()
      .HasOne(r => r.User);
  }
}
