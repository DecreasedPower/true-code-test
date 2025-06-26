using FinanceService.Models.Dto.Models;

namespace FinanceService.Business.Commands.Interfaces;

public interface IGetCurrenciesCommand
{
  Task<List<Currency>> ExecuteAsync(string currencyCode = null, CancellationToken ct = default);
}
