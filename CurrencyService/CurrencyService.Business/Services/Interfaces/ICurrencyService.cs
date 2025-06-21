using CurrencyService.Business.Models;

namespace CurrencyService.Business.Services.Interfaces;

public interface ICurrencyService
{
  Task UpdateCurrencyRate(List<Currency> currencies, CancellationToken ct);
}
