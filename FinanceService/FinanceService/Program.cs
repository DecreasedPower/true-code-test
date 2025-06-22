using System.Security.Cryptography;
using System.Text;
using FinanceService.Business.Commands;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Data.Repositories;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

    builder.Services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();
    builder.Services.AddTransient<IAddValuteCommand, AddValuteCommand>();
    builder.Services.AddTransient<IGetValutesCommand, GetValutesCommand>();
    builder.Services.AddTransient<IUpdateValutesCommand, UpdateValutesCommand>();
    builder.Services.AddTransient<IRemoveValuteCommand, RemoveValuteCommand>();

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
    builder.Services.AddAuthorization();

    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
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
