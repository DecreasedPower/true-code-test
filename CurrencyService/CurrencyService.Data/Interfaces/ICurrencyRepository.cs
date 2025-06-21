using CurrencyService.Db.Models;

namespace CurrencyService.Data.Interfaces;

public interface ICurrencyRepository
{
  Task UpdateCurrencies(List<DbCurrency> currencies);
}
