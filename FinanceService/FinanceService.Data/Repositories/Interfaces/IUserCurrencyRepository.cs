using FinanceService.Db.Models;
using FinanceService.Models.Dto.Models;

namespace FinanceService.Data.Repositories.Interfaces;

public interface IUserCurrencyRepository
{
  Task<List<Currency>> GetAsync(int userId, CancellationToken ct = default);
  Task AddAsync(DbUserCurrency userCurrency, CancellationToken ct = default);
  Task UpdateAsync(int userId, List<DbUserCurrency> userCurrencies, CancellationToken ct = default);
  Task<int> RemoveAsync(DbUserCurrency userCurrency, CancellationToken ct = default);
}
