using System.Security.Claims;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FinanceService.Business.Commands;

public class AddCurrencyCommand(
  IUserCurrencyRepository repository,
  HttpContextAccessor contextAccessor,
  ILogger<AddCurrencyCommand> logger)
  : IAddCurrencyCommand
{
  public async Task<bool> ExecuteAsync(string currencyCode, CancellationToken ct)
  {
    var userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    try
    {
      await repository.AddAsync(new DbUserCurrency
      {
        UserId = userId,
        CurrencyId = currencyCode
      }, ct);
    }
    catch (Exception e)
    {
      logger.LogWarning(e, "Failed to add user currency.");
      return false;
    }

    return true;
  }
}
