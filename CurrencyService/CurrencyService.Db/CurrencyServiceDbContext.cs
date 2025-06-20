using CurrencyService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Db;

public class CurrencyServiceDbContext(DbContextOptions<CurrencyServiceDbContext> options) : DbContext(options)
{
  public DbSet<Currency> Currencies { get; set; }
}
