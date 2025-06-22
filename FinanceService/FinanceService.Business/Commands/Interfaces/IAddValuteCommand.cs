namespace FinanceService.Business.Commands.Interfaces;

public interface IAddValuteCommand
{
  Task<bool> ExecuteAsync(string valuteCode, CancellationToken ct);
}
