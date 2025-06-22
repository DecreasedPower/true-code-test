using FinanceService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Db;

public class FinanceServiceDbContext(DbContextOptions<FinanceServiceDbContext> options) : DbContext(options)
{
  public DbSet<DbUserCurrency> UserCurrencies { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
  }
}
