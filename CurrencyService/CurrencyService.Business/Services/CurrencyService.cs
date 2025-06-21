using CurrencyService.Business.Models;
using CurrencyService.Business.Services.Interfaces;
using CurrencyService.Data.Interfaces;

namespace CurrencyService.Business.Services;

public class CurrencyService(
  ICurrencyRepository repository)
  : ICurrencyService
{
  public Task UpdateCurrencyRate(List<Currency> currencies, CancellationToken ct)
  {
    if (currencies is null || currencies.Count == 0)
    {
      return Task.CompletedTask;
    }

    return repository.UpdateCurrencies(currencies.ConvertAll(v => v.Map()));
  }
}
