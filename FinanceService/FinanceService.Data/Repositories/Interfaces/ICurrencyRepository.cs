using FinanceService.Models.Dto.Models;

namespace FinanceService.Data.Repositories.Interfaces;

public interface ICurrencyRepository
{
  Task<List<Currency>> GetAvailableCurrenciesAsync(CancellationToken ct = default);
  Task<Currency> GetAsync(string currencyCode, CancellationToken ct = default);
}
