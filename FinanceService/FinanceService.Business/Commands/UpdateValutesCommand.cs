using FinanceService.Business.Commands.Interfaces;

namespace FinanceService.Business.Commands;

public class UpdateValutesCommand : IUpdateValutesCommand
{
  public Task<bool> ExecuteAsync(List<string> valuteCodes, CancellationToken ct)
  {
    throw new NotImplementedException();
  }
}
