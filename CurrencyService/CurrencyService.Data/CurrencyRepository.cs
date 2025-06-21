using CurrencyService.Data.Interfaces;
using CurrencyService.Db;
using CurrencyService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Data;

public class CurrencyRepository(
  CurrencyServiceDbContext dbContext)
  : ICurrencyRepository
{
  public async Task UpdateCurrencies(List<DbCurrency> currencies)
  {
    await dbContext.Currency.ExecuteDeleteAsync();
    dbContext.Currency.AddRange(currencies);
    await dbContext.SaveChangesAsync();
  }
}
