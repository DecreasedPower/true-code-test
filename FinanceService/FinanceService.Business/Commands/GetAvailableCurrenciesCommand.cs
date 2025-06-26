using FinanceService.Business.Commands.Interfaces;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Models.Dto.Models;

namespace FinanceService.Business.Commands;

public class GetAvailableCurrenciesCommand(
  ICurrencyRepository repository)
  : IGetAvailableCurrenciesCommand
{
  public Task<List<Currency>> ExecuteAsync(CancellationToken ct)
  {
    return repository.GetAvailableCurrencies(ct);
  }
}
