using FinanceService.Models.Dto.Models;

namespace FinanceService.Business.Commands.Interfaces;

public interface IGetAvailableCurrenciesCommand
{
  Task<List<Currency>> ExecuteAsync(CancellationToken ct);
}
