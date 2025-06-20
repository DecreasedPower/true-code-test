using CurrencyService.Db.Models;

namespace CurrencyService.Data.Interfaces;

public interface ICurrencyRepository
{
  Task<bool> UpdateCurrencies(List<Currency> currencies);
}
