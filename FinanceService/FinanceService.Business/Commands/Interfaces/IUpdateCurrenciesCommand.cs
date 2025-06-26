namespace FinanceService.Business.Commands.Interfaces;

public interface IUpdateCurrenciesCommand
{
  Task<bool> ExecuteAsync(List<string> currencyCodes, CancellationToken ct);
}
