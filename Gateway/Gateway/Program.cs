using Gateway.Models.Configs;

namespace Gateway;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    var jwtOptions = builder.Configuration.GetSection("ServiceConfiguration");
    builder.Services.Configure<ServiceConfiguration>(jwtOptions);
    builder.Services.AddHttpClient();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseAuthorization();
    app.MapControllers();
    app.Run();
  }
}
