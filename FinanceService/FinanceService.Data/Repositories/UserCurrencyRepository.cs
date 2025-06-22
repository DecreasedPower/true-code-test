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
  public Task<List<UserValute>> GetAsync(List<string> valuteCodes, CancellationToken ct = default)
  {
    var userCurrencies = dbContext.UserCurrencies
      .AsNoTracking();

    if (valuteCodes is not null && valuteCodes.Count > 0)
    {
      userCurrencies = userCurrencies
        .Where(uc => valuteCodes.Contains(uc.CurrencyId));
    }

    return userCurrencies
      .Select(uc => new UserValute(uc.UserId, uc.CurrencyId))
      .ToListAsync(ct);
  }

  public Task AddAsync(DbUserCurrency userCurrency, CancellationToken ct = default)
  {
    dbContext.UserCurrencies.Add(userCurrency);
    return dbContext.SaveChangesAsync(ct);
  }

  public async Task UpdateAsync(List<DbUserCurrency> userCurrencies, CancellationToken ct = default)
  {
    var sqlSb = new StringBuilder(@"
      BEGIN;
        DELETE FROM ""Currencies"" WHERE 1 = 1;");

    if (userCurrencies is not null && userCurrencies.Count > 0)
    {
      sqlSb.Append(@"
        INSERT INTO ""UserCurrencies"" (""UserId"", ""Code"")
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
