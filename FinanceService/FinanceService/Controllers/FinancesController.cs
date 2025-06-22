using System.ComponentModel.DataAnnotations;
using FinanceService.Business.Commands.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.Controllers;

[ApiController]
[Route("currencies")]
public class FinancesController : Controller
{
  [HttpPost]
  public async Task<IActionResult> AddAsync(
    [Required][FromBody] string valuteCode,
    [FromServices] IAddValuteCommand command,
    CancellationToken ct)
  {
    var result = await command.ExecuteAsync(valuteCode, ct);
    return Ok(result);
  }

  [HttpGet]
  [Authorize]
  public Task<IActionResult> GetAllAsync(
    [FromServices] IGetValutesCommand command,
    CancellationToken ct)
  {
    throw new NotImplementedException();
  }

  [HttpGet("{valuteCode}")]
  public Task<IActionResult> GetAsync(
    [FromRoute][Required][MaxLength(100)] string valuteCode,
    [FromServices] IGetValutesCommand command,
    CancellationToken ct)
  {
    throw new NotImplementedException();
  }

  [HttpPut]
  public Task<IActionResult> UpdateAsync(
    [Required] List<string> valuteCodes,
    [FromServices] IUpdateValutesCommand command,
    CancellationToken ct)
  {
    throw new NotImplementedException();
  }

  [HttpDelete("{valuteCode}")]
  public Task<IActionResult> UpdateAsync(
    [Required] string valuteCode,
    [FromServices] IRemoveValuteCommand command,
    CancellationToken ct)
  {
    throw new NotImplementedException();
  }
}
