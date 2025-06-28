using FinanceService.Business.Commands.Interfaces;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace FinanceService.gRPC.Services;

[Authorize]
public class CurrencyGrpcService(
  IGetAvailableCurrenciesCommand getAvailableCommand,
  IAddCurrencyCommand addCurrencyCommand,
  IGetCurrenciesCommand getCurrenciesCommand,
  IUpdateCurrenciesCommand updateCurrenciesCommand,
  IRemoveCurrencyCommand removeCurrencyCommand)
  : CurrencyService.CurrencyServiceBase
{
  public override async Task<CurrencyList> GetAvailableCurrencies(Empty request, ServerCallContext context)
  {
    var currencies = await getAvailableCommand.ExecuteAsync(context.CancellationToken);
    return new CurrencyList
    {
      Currencies = { currencies.Select(MapToProto) }
    };
  }

  public override async Task<OperationResult> AddCurrency(CurrencyCodeRequest request, ServerCallContext context)
  {
    bool added = await addCurrencyCommand.ExecuteAsync(request.CurrencyCode, context.CancellationToken);
    return new OperationResult
    {
      Success = added
    };
  }

  public override async Task<CurrencyList> GetAllCurrencies(Empty request, ServerCallContext context)
  {
    var currencies = await getCurrenciesCommand.ExecuteAsync(ct: context.CancellationToken);
    return new CurrencyList
    {
      Currencies = { currencies.Select(MapToProto) }
    };
  }

  public override async Task<Currency> GetCurrency(CurrencyCodeRequest request, ServerCallContext context)
  {
    var currency = await getCurrenciesCommand.ExecuteAsync(request.CurrencyCode, context.CancellationToken);
    if (currency is null || currency.Count == 0)
    {
      return null;
    }

    return new Currency
    {
      Code = currency[0].Code,
      Name = currency[0].Name,
      Rate = currency[0].Rate
    };
  }

  public override async Task<OperationResult> UpdateCurrencies(CurrencyCodeList request, ServerCallContext context)
  {
    var currencyCodes = request.CurrencyCodes.Select(cc => cc).ToList();

    bool updated = await updateCurrenciesCommand.ExecuteAsync(currencyCodes, context.CancellationToken);

    return new OperationResult
    {
      Success = updated
    };
  }

  public override async Task<OperationResult> RemoveCurrency(CurrencyCodeRequest request, ServerCallContext context)
  {
    bool removed = await removeCurrencyCommand.ExecuteAsync(request.CurrencyCode, context.CancellationToken);

    return new OperationResult
    {
      Success = removed
    };
  }

  private static Currency MapToProto(Models.Dto.Models.Currency entity)
  {
    return new Currency
    {
      Code = entity.Code,
      Name = entity.Name,
      Rate = entity.Rate
    };
  }
}
