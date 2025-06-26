using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db;
using FinanceService.Models.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Data.Repositories;

public class CurrencyRepository(
  FinanceServiceDbContext dbContext) : ICurrencyRepository
{
  public Task<List<Currency>> GetAvailableCurrencies(CancellationToken ct = default)
  {
    return dbContext.Currencies
      .AsNoTracking()
      .Select(c => new Currency(c.Id, c.Name, c.Rate))
      .ToListAsync(ct);
  }
}
