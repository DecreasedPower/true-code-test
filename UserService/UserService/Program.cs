using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UserService.Business.Commands;
using UserService.Business.Commands.Interfaces;
using UserService.Data.Repositories;
using UserService.Data.Repositories.Interfaces;
using UserService.Db;
using UserService.Models.Dto.Configs;

namespace UserService;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<UserServiceDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));

    var jwtOptions = builder.Configuration.GetSection("Jwt");
    builder.Services.Configure<JwtOptions>(jwtOptions);
    builder.Services.Configure<JwtOptions>(options =>
    {
      options.PrivateKey = CreateRsaFromBase64(options.PrivateKeyBase64);
    });

    builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddTransient<ILoginCommand, LoginCommand>();
    builder.Services.AddTransient<IRefreshCommand, RefreshCommand>();
    builder.Services.AddTransient<IRegisterCommand, RegisterCommand>();
    builder.Services.AddTransient<ILogoutCommand, LogoutCommand>();
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.MapControllers();
    app.UseRouting();
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
