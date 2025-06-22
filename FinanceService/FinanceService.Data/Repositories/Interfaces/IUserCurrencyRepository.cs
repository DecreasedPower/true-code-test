using FinanceService.Db.Models;
using FinanceService.Models.Dto.Models;

namespace FinanceService.Data.Repositories.Interfaces;

public interface IUserCurrencyRepository
{
  Task<List<UserValute>> GetAsync(List<string> valuteCodes, CancellationToken ct = default);
  Task AddAsync(DbUserCurrency userCurrency, CancellationToken ct = default);
  Task UpdateAsync(List<DbUserCurrency> userCurrencies, CancellationToken ct = default);
  Task<int> RemoveAsync(DbUserCurrency userCurrency, CancellationToken ct = default);
}
