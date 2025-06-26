using FinanceService.Models.Dto.Models;

namespace FinanceService.Data.Repositories.Interfaces;

public interface ICurrencyRepository
{
  Task<List<Currency>> GetAvailableCurrencies(CancellationToken ct = default);
}
