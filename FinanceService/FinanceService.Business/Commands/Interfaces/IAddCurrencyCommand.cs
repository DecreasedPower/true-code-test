namespace FinanceService.Business.Commands.Interfaces;

public interface IAddCurrencyCommand
{
  Task<bool> ExecuteAsync(string currencyCode, CancellationToken ct);
}
