using System.Security.Claims;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FinanceService.Business.Commands;

public class UpdateCurrenciesCommand(
  IUserCurrencyRepository repository,
  IHttpContextAccessor contextAccessor,
  ILogger<UpdateCurrenciesCommand> logger)
  : IUpdateCurrenciesCommand
{
  public async Task<bool> ExecuteAsync(List<string> currencyCodes, CancellationToken ct)
  {
    var userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    try
    {
      await repository.UpdateAsync(
        userId,
        currencyCodes.ConvertAll(vc =>
        new DbUserCurrency
        {
          CurrencyId = vc,
          UserId = userId
        }), ct);
    }
    catch (Exception e)
    {
      logger.LogWarning(e, "Failed to update user currencies.");
      return false;
    }

    return true;
  }
}
