using System.Security.Claims;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Models.Dto.Models;
using Microsoft.AspNetCore.Http;

namespace FinanceService.Business.Commands;

public class GetCurrenciesCommand(
  IUserCurrencyRepository repository,
  HttpContextAccessor contextAccessor)
  : IGetCurrenciesCommand
{
  public Task<List<Currency>> ExecuteAsync(string currencyCode = null, CancellationToken ct = default)
  {
    var userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    List<string> currencies = null;
    if (!string.IsNullOrEmpty(currencyCode))
    {
      currencies = [currencyCode];
    }

    return repository.GetAsync(userId, currencies, ct);
  }
}
