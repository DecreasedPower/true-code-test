using System.ComponentModel.DataAnnotations;
using FinanceService.Business.Commands.Interfaces;
using FinanceService.Models.Dto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
[Authorize]
[ApiController]
[Route("currencies")]
public class CurrenciesController : Controller
{
  [HttpPost]
  [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
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

    return Created("currencies", true);
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<CurrencyDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetAllAsync(
    [FromServices] IGetCurrenciesCommand command,
    CancellationToken ct)
  {
    List<CurrencyDto> currencies = await command.ExecuteAsync(null, ct);
    return Ok(currencies);
  }

  [HttpGet("{currencyCode}")]
  [ProducesResponseType(typeof(CurrencyDto), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetAsync(
    [FromRoute][Required][MaxLength(100)] string currencyCode,
    [FromServices] IGetCurrenciesCommand command,
    CancellationToken ct)
  {
    List<CurrencyDto> currencies = await command.ExecuteAsync(currencyCode, ct);
    if (currencies.Count == 0)
    {
      return NotFound("Specified currency not found.");
    }

    return Ok(currencies[0]);
  }

  [HttpGet("available")]
  [ProducesResponseType(typeof(List<CurrencyDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetAvailableAsync(
    [FromServices] IGetAvailableCurrenciesCommand command,
    CancellationToken ct)
  {
    var result = await command.ExecuteAsync(ct);
    return Ok(result);
  }

  [HttpPut]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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
