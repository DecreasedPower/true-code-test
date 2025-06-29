using FinanceService.Models.Dto.Models;

namespace FinanceService.Business.Commands.Interfaces;

public interface IGetCurrenciesCommand
{
  Task<List<CurrencyDto>> ExecuteAsync(string currencyCode = null, CancellationToken ct = default);
}
