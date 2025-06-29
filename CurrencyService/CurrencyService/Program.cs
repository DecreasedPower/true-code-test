using CurrencyService.Business.BackgroundServices;
using CurrencyService.Business.Services.Interfaces;
using CurrencyService.Data;
using CurrencyService.Data.Interfaces;
using CurrencyService.Db;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<CurrencyServiceDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));

    builder.Services.AddTransient<ICurrencyService, Business.Services.CurrencyService>();
    builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
    builder.Services.AddHostedService<CurrencyRateUpdateService>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
      var db = scope.ServiceProvider.GetRequiredService<CurrencyServiceDbContext>();

      const int maxRetries = 10;
      int retries = 0;

      while (true)
      {
        try
        {
          db.Database.Migrate();
          break;
        }
        catch (Exception) when (retries < maxRetries)
        {
          retries++;
          Thread.Sleep(2000);
        }
      }
    }

    app.Run();
  }
}
