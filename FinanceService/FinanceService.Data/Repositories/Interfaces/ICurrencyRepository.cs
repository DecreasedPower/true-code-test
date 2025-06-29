using FinanceService.Models.Dto.Models;

namespace FinanceService.Data.Repositories.Interfaces;

public interface ICurrencyRepository
{
  Task<List<CurrencyDto>> GetAvailableCurrenciesAsync(CancellationToken ct = default);
  Task<CurrencyDto> GetAsync(string currencyCode, CancellationToken ct = default);
}
