using FinanceService.Business.Commands.Interfaces;

namespace FinanceService.Business.Commands;

public class RemoveValuteCommand : IRemoveValuteCommand
{
  public Task<bool> ExecuteAsync(string valuteCode, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}
