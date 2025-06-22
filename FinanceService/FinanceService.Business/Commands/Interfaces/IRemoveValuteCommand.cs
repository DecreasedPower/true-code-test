namespace FinanceService.Business.Commands.Interfaces;

public interface IRemoveValuteCommand
{
  Task<bool> ExecuteAsync(string valuteCode, CancellationToken ct = default);
}
