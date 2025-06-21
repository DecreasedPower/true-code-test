using CurrencyService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Db;

public class CurrencyServiceDbContext(DbContextOptions<CurrencyServiceDbContext> options) : DbContext(options)
{
  public DbSet<DbCurrency> Currency { get; set; }
  public DbSet<DbUser> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<DbUser>()
      .HasMany(u => u.Currencies)
      .WithMany(c => c.Users)
      .UsingEntity(uc => uc.ToTable("UserCurrencies"));
  }
}
