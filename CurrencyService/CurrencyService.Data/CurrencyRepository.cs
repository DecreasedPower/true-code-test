using System.Text;
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
    var sqlSb = new StringBuilder(@"
      BEGIN;
        DELETE FROM ""Currencies"" WHERE 1 = 1;
        INSERT INTO ""Currencies"" (""Id"", ""Name"", ""Rate"")
        VALUES ");

    for (int i = 0; i < currencies.Count; i++)
    {
      if (i == currencies.Count - 1)
      {
        sqlSb.Append($"('{currencies[i].Id}', '{currencies[i].Name}', '{currencies[i].Rate}'); COMMIT;");
      }
      else
      {
        sqlSb.Append($"('{currencies[i].Id}', '{currencies[i].Name}', '{currencies[i].Rate}'),");
      }
    }

    await dbContext.Database.ExecuteSqlRawAsync(sqlSb.ToString());
  }
}
