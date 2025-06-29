using FinanceService.Models.Dto.Models;

namespace FinanceService.Business.Commands.Interfaces;

public interface IGetAvailableCurrenciesCommand
{
  Task<List<CurrencyDto>> ExecuteAsync(CancellationToken ct);
}
