using FinanceService.Business.Commands.Interfaces;
using FinanceService.Models.Dto.Models;

namespace FinanceService.Business.Commands;

public class GetValutesCommand : IGetValutesCommand
{
  public Task<List<UserValute>> ExecuteAsync(string valuteCode = null, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}
