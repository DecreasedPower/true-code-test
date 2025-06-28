using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using FinanceService.Business.Commands;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Data.Repositories;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db;
using FinanceService.gRPC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinanceService;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<FinanceServiceDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();
    builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
    builder.Services.AddTransient<IAddCurrencyCommand, AddCurrencyCommand>();
    builder.Services.AddTransient<IGetCurrenciesCommand, GetCurrenciesCommand>();
    builder.Services.AddTransient<IGetAvailableCurrenciesCommand, GetAvailableCurrenciesCommand>();
    builder.Services.AddTransient<IUpdateCurrenciesCommand, UpdateCurrenciesCommand>();
    builder.Services.AddTransient<IRemoveCurrencyCommand, RemoveCurrencyCommand>();

    var jwtOptions = builder.Configuration.GetSection("Jwt");
    builder.Services
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(opt =>
      {
        var rsa = CreateRsaFromBase64(jwtOptions["PublicKeyBase64"]);
        opt.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidIssuer = jwtOptions["Issuer"],
          ValidateAudience = true,
          ValidAudience = jwtOptions["Audience"],
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new RsaSecurityKey(rsa)
        };
      });

    var servicePort = builder.Configuration.GetValue<int>("ServicePort");
    builder.WebHost.ConfigureKestrel(options =>
    {
      options.ListenAnyIP(servicePort, listenOptions =>
      {
        listenOptions.Protocols = HttpProtocols.Http2;
      });
    });

    builder.Services.AddGrpc();
    builder.Services.AddAuthorization();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapGrpcService<CurrencyGrpcService>();
    app.Run();
  }

  private static RSA CreateRsaFromBase64(string base64)
  {
    var key = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
    var rsa = RSA.Create();
    rsa.ImportFromPem(key);
    return rsa;
  }
}
