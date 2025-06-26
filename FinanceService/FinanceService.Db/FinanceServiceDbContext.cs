using FinanceService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Db;

public class FinanceServiceDbContext(DbContextOptions<FinanceServiceDbContext> options) : DbContext(options)
{
  public DbSet<DbUserCurrency> UserCurrencies { get; set; }
  public DbSet<DbCurrency> Currencies { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<DbUserCurrency>()
      .HasOne(uc => uc.Currency)
      .WithMany(c => c.UsersCurrencies);
  }
}
