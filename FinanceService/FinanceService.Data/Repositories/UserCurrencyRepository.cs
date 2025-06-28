using System.Text;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db;
using FinanceService.Db.Models;
using FinanceService.Models.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Data.Repositories;

public class UserCurrencyRepository(
  FinanceServiceDbContext dbContext)
  : IUserCurrencyRepository
{
  public Task<List<Currency>> GetAsync(int userId, List<string> currencyCodes = null, CancellationToken ct = default)
  {
    var userCurrencies = dbContext.UserCurrencies
      .AsNoTracking()
      .Where(uc => uc.UserId == userId);

    if (currencyCodes is not null && currencyCodes.Count > 0)
    {
      userCurrencies = userCurrencies
        .Where(uc => currencyCodes.Contains(uc.CurrencyId));
    }

    return userCurrencies
      .Include(uc => uc.Currency)
      .Select(uc => new Currency(uc.Currency.Id, uc.Currency.Name, uc.Currency.Rate))
      .ToListAsync(ct);
  }

  public Task AddAsync(DbUserCurrency userCurrency, CancellationToken ct = default)
  {
    dbContext.UserCurrencies.Add(userCurrency);
    return dbContext.SaveChangesAsync(ct);
  }

  public async Task UpdateAsync(int userId, List<DbUserCurrency> userCurrencies, CancellationToken ct = default)
  {
    var sqlSb = new StringBuilder(@$"
      BEGIN TRANSACTION;
        DELETE FROM ""UsersCurrencies"" WHERE ""UserId"" = {userId};");

    if (userCurrencies is not null && userCurrencies.Count > 0)
    {
      sqlSb.Append(@"
        INSERT INTO ""UsersCurrencies"" (""UserId"", ""CurrencyId"")
        VALUES ");

      for (int i = 0; i < userCurrencies.Count; i++)
      {
        if (i == userCurrencies.Count - 1)
        {
          sqlSb.Append($"({userCurrencies[i].UserId}, '{userCurrencies[i].CurrencyId}');");
        }
        else
        {
          sqlSb.Append($"({userCurrencies[i].UserId}, '{userCurrencies[i].CurrencyId}'), ");
        }
      }
    }

    sqlSb.Append(" COMMIT;");

    await dbContext.Database.ExecuteSqlRawAsync(sqlSb.ToString(), ct);
  }

  public Task<int> RemoveAsync(DbUserCurrency userCurrency, CancellationToken ct = default)
  {
    var userCurrencies = dbContext.UserCurrencies
      .Where(uc =>
        uc.UserId == userCurrency.UserId &&
        uc.CurrencyId == userCurrency.CurrencyId);

    return userCurrencies.ExecuteDeleteAsync(ct);
  }
}
