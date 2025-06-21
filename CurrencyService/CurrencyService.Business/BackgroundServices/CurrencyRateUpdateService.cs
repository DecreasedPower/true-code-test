using System.Text;
using System.Xml.Serialization;
using CurrencyService.Business.Models;
using CurrencyService.Business.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyService.Business.BackgroundServices;

public class CurrencyRateUpdateService(
  IServiceProvider serviceProvider,
  ILogger<CurrencyRateUpdateService> logger)
  : IHostedService, IDisposable
{
  private Timer _timer;

  public Task StartAsync(CancellationToken ct)
  {
    _timer = new Timer(async _ => await Run(ct), null, TimeSpan.Zero, TimeSpan.FromHours(1));
    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken ct)
  {
    _timer.Change(Timeout.Infinite, 0);
    return Task.CompletedTask;
  }

  public void Dispose()
  {
    _timer.Dispose();
  }

  private async Task Run(CancellationToken ct)
  {
    logger.LogInformation("Getting currency rates.");

    Stream xml;
    using (var httpClient = new HttpClient())
    {
      xml = await httpClient.GetStreamAsync("http://www.cbr.ru/scripts/XML_daily.asp", ct);
    }

    var serializer = new XmlSerializer(typeof(CurrencyRates));
    List<Currency> currencies;

    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    using (TextReader reader = new StreamReader(xml, Encoding.GetEncoding("windows-1251")))
    {
      var currencyRates = (CurrencyRates)serializer.Deserialize(reader);

      currencies = currencyRates?.Currencies;
    }

    IServiceScope scope = null;
    try
    {
      scope = serviceProvider.CreateScope();

      var service = scope.ServiceProvider.GetRequiredService<ICurrencyService>();

      await service.UpdateCurrencyRate(currencies, ct);
    }
    catch (Exception e)
    {
      logger.LogError(e, "Failed to update currency rates.");
    }
    finally
    {
      scope?.Dispose();
    }
  }
}
