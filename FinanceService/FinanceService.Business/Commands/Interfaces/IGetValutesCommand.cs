using FinanceService.Models.Dto.Models;

namespace FinanceService.Business.Commands.Interfaces;

public interface IGetValutesCommand
{
  Task<List<UserValute>> ExecuteAsync(string valuteCode = null, CancellationToken ct = default);
}
