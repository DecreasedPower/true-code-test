using System.Security.Claims;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Models.Dto.Models;
using Microsoft.AspNetCore.Http;

namespace FinanceService.Business.Commands;

public class GetCurrenciesCommand(
  IUserCurrencyRepository userCurrencyRepository,
  ICurrencyRepository currencyRepository,
  IHttpContextAccessor contextAccessor)
  : IGetCurrenciesCommand
{
  public async Task<List<CurrencyDto>> ExecuteAsync(string currencyCode = null, CancellationToken ct = default)
  {
    var userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

    if (!string.IsNullOrEmpty(currencyCode))
    {
      var currency = await currencyRepository.GetAsync(currencyCode, ct);
      return [currency];
    }

    return await userCurrencyRepository.GetAsync(userId, ct);
  }
}
