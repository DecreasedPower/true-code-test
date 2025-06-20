using CurrencyService.Data.Interfaces;
using CurrencyService.Db;
using CurrencyService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Data;

public class CurrencyRepository(
  CurrencyServiceDbContext dbContext)
  : ICurrencyRepository
{
  public async Task<bool> UpdateCurrencies(List<Currency> currencies)
  {
    await dbContext.Currencies.ExecuteDeleteAsync();
    dbContext.Currencies.AddRange(currencies);
    await dbContext.SaveChangesAsync();

    return true;
  }
}
