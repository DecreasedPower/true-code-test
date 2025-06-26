using System.ComponentModel.DataAnnotations;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Models.Dto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.Controllers;

[Authorize]
[ApiController]
[Route("currencies")]
public class CurrenciesController : Controller
{
  [HttpPost]
  public async Task<IActionResult> AddAsync(
    [Required][FromBody] string currencyCode,
    [FromServices] IAddCurrencyCommand command,
    CancellationToken ct)
  {
    bool result = await command.ExecuteAsync(currencyCode, ct);
    if (!result)
    {
      return BadRequest("Failed to add currency.");
    }

    return Created("Currencies", true);
  }

  [HttpGet]
  public async Task<IActionResult> GetAllAsync(
    [FromServices] IGetCurrenciesCommand command,
    CancellationToken ct)
  {
    List<Currency> currencies = await command.ExecuteAsync(null, ct);
    return Ok(currencies);
  }

  [HttpGet("{currencyCode}")]
  public async Task<IActionResult> GetAsync(
    [FromRoute][Required][MaxLength(100)] string currencyCode,
    [FromServices] IGetCurrenciesCommand command,
    CancellationToken ct)
  {
    List<Currency> currencies = await command.ExecuteAsync(currencyCode, ct);
    if (currencies.Count == 0)
    {
      return NotFound("Specified currency not found.");
    }

    return Ok(currencies[0]);
  }

  [HttpGet("available")]
  public async Task<IActionResult> GetAvailableAsync(
    [FromServices] IGetAvailableCurrenciesCommand command,
    CancellationToken ct)
  {
    var result = await command.ExecuteAsync(ct);
    return Ok(result);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateAsync(
    [Required] List<string> currencyCodes,
    [FromServices] IUpdateCurrenciesCommand command,
    CancellationToken ct)
  {
    bool result = await command.ExecuteAsync(currencyCodes, ct);
    if (!result)
    {
      return BadRequest("Failed to update currencies.");
    }

    return Ok(true);
  }

  [HttpDelete("{currencyCode}")]
  public async Task<IActionResult> UpdateAsync(
    [Required] string currencyCode,
    [FromServices] IRemoveCurrencyCommand command,
    CancellationToken ct)
  {
    bool result = await command.ExecuteAsync(currencyCode, ct);
    if (!result)
    {
      return NotFound("Specified currency not found.");
    }

    return Ok(true);
  }
}
