namespace FinanceService.Business.Commands.Interfaces;

public interface IUpdateValutesCommand
{
  Task<bool> ExecuteAsync(List<string> valuteCodes, CancellationToken ct);
}
