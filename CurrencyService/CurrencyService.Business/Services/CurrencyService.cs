using System.Xml.Serialization;
using CurrencyService.Business.Models;
using CurrencyService.Business.Services.Interfaces;

namespace CurrencyService.Business.Services;

public class CurrencyService : ICurrencyService
{
  public Task UpdateCurrencyRate(List<Valute> valutes, CancellationToken ct)
  {
    throw new NotImplementedException();
  }
}
