using CurrencyService.Business.BackgroundServices;
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

    // Add services to the container.
    builder.Services.AddHostedService<CurrencyRateUpdateService>();

    var app = builder.Build();

    app.Run();
  }
}
