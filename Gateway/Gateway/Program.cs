using System.Security.Cryptography;
using System.Text;
using FinanceService.gRPC;
using Gateway.Models.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Gateway;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

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

    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

    var serviceConfiguration = builder.Configuration.GetSection("ServiceConfiguration");
    builder.Services.AddGrpcClient<CurrencyService.CurrencyServiceClient>(options =>
      {
        options.Address = new Uri(serviceConfiguration["FinanceService"]);
      })
      .ConfigureChannel(options =>
      {
        options.HttpHandler = new SocketsHttpHandler
        {
          SslOptions = { RemoteCertificateValidationCallback = (_, _, _, _) => true }
        };
      });

    builder.Services.Configure<ServiceConfiguration>(serviceConfiguration);
    builder.Services.AddHttpClient();
    builder.Services.AddControllers();
    builder.Services.AddAuthorization();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.MapControllers();
    app.UseAuthorization();
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
