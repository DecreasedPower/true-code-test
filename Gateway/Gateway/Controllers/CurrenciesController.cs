using System.ComponentModel.DataAnnotations;
using FinanceService.gRPC;
using Gateway.Models.Configs;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Currency = Gateway.Models.Currency;

namespace Gateway.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
[Authorize]
[ApiController]
[Route("currencies")]
public class CurrenciesController(
  IOptions<ServiceConfiguration> serviceConfiguration)
  : Controller
{
  private readonly string _grpcEndpoint = serviceConfiguration.Value.FinanceService;

  [HttpPost]
  [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> AddAsync(
    [Required][FromBody] string currencyCode,
    CancellationToken ct)
  {
    string token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
    var metadata = new Metadata
    {
      { "Authorization", token }
    };

    using var channel = GrpcChannel.ForAddress(_grpcEndpoint, new GrpcChannelOptions
    {
      HttpHandler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      }});

    var client = new CurrencyService.CurrencyServiceClient(channel);

    var response = await client.AddCurrencyAsync(
      new CurrencyCodeRequest { CurrencyCode = currencyCode },
      metadata,
      cancellationToken: ct);

    if (!response.Success)
    {
      return BadRequest(response.Error);
    }

    return Ok(response.Success);
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<Currency>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetAllAsync(CancellationToken ct)
  {
    string token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
    var metadata = new Metadata
    {
      { "Authorization", token }
    };

    using var channel = GrpcChannel.ForAddress(_grpcEndpoint, new GrpcChannelOptions
    {
      HttpHandler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      }});

    var client = new CurrencyService.CurrencyServiceClient(channel);

    var response = await client.GetAllCurrenciesAsync(new Empty(), metadata, cancellationToken: ct);

    return Ok(response.Currencies);
  }

  [HttpGet("{currencyCode}")]
  [ProducesResponseType(typeof(Currency), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetAsync(
    [FromRoute][Required][MaxLength(100)] string currencyCode,
    CancellationToken ct)
  {
    string token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
    var metadata = new Metadata
    {
      { "Authorization", token }
    };

    using var channel = GrpcChannel.ForAddress(_grpcEndpoint, new GrpcChannelOptions
    {
      HttpHandler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      }});

    var client = new CurrencyService.CurrencyServiceClient(channel);

    var currency = await client.GetCurrencyAsync(
      new CurrencyCodeRequest { CurrencyCode = currencyCode},
      metadata,
      cancellationToken: ct);

    if (currency is null)
    {
      return NotFound("Specified currency not found.");
    }

    return Ok(currency);
  }

  [HttpGet("available")]
  [ProducesResponseType(typeof(List<Currency>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetAvailableAsync(CancellationToken ct)
  {
    string token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
    var metadata = new Metadata
    {
      { "Authorization", token }
    };

    using var channel = GrpcChannel.ForAddress(_grpcEndpoint, new GrpcChannelOptions
    {
      HttpHandler = new HttpClientHandler
      {
        AllowAutoRedirect = false,
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      }});

    var client = new CurrencyService.CurrencyServiceClient(channel);

    var response = await client.GetAvailableCurrenciesAsync(new Empty(), metadata, cancellationToken: ct);

    return Ok(response.Currencies);
  }

  [HttpPut]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> UpdateAsync(
    [Required] List<string> currencyCodes,
    CancellationToken ct)
  {
    string token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
    var metadata = new Metadata
    {
      { "Authorization", token }
    };

    using var channel = GrpcChannel.ForAddress(_grpcEndpoint, new GrpcChannelOptions
    {
      HttpHandler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      }});

    var client = new CurrencyService.CurrencyServiceClient(channel);

    var response = await client.UpdateCurrenciesAsync(
      new CurrencyCodeList { CurrencyCodes = { currencyCodes }},
      metadata,
      cancellationToken: ct);

    if (!response.Success)
    {
      return BadRequest(response.Error);
    }

    return Ok(response.Success);
  }

  [HttpDelete("{currencyCode}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> UpdateAsync(
    [Required] string currencyCode,
    CancellationToken ct)
  {
    string token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
    var metadata = new Metadata
    {
      { "Authorization", token }
    };

    using var channel = GrpcChannel.ForAddress(_grpcEndpoint, new GrpcChannelOptions
    {
      HttpHandler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      }});

    var client = new CurrencyService.CurrencyServiceClient(channel);

    var response = await client.RemoveCurrencyAsync(
      new CurrencyCodeRequest { CurrencyCode = currencyCode },
      metadata,
      cancellationToken: ct);

    if (!response.Success)
    {
      return NotFound(response.Error);
    }

    return Ok(response.Success);
  }
}
