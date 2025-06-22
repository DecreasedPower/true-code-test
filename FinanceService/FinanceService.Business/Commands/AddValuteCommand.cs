using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db.Models;
using Microsoft.AspNetCore.Http;

namespace FinanceService.Business.Commands;

public class AddValuteCommand(
  IUserCurrencyRepository repository,
  IHttpContextAccessor contextAccessor)
  : IAddValuteCommand
{
  public async Task<bool> ExecuteAsync(string valuteCode, CancellationToken ct)
  {
    var userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    try
    {
      await repository.AddAsync(new DbUserCurrency
      {
        UserId = userId,
        CurrencyId = valuteCode
      });
    }
    catch (Exception e)
    {
      throw new BadHttpRequestException("Incorrect valute code provided.");
    }

    return true;
  }
}
