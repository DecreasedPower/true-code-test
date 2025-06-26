namespace FinanceService.Business.Commands.Interfaces;

public interface IRemoveCurrencyCommand
{
  Task<bool> ExecuteAsync(string currencyCode, CancellationToken ct = default);
}
