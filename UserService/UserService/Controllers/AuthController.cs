using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UserService.Business.Commands.Interfaces;
using UserService.Models.Dto.Responses;
using LoginRequest = UserService.Models.Dto.Requests.LoginRequest;

namespace UserService.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
[Route("auth")]
public class AuthController : Controller
{
  [HttpPost("register")]
  [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Register(
    [FromBody][Required] LoginRequest request,
    [FromServices] IRegisterCommand command,
    CancellationToken ct)
  {
    AuthResponse result = await command.ExecuteAsync(request, ct);
    if (result is null)
    {
      return Unauthorized("Failed to register.");
    }

    return Ok(result);
  }

  [HttpPost("login")]
  [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Login(
    [FromBody][Required] LoginRequest request,
    [FromServices] ILoginCommand command,
    CancellationToken ct)
  {
    AuthResponse result = await command.ExecuteAsync(request, ct);
    if (result is null)
    {
      return Unauthorized("Failed to login.");
    }

    return Ok(result);
  }

  [HttpPost("refresh")]
  [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Refresh(
    [FromBody][Required] string refreshToken,
    [FromServices] IRefreshCommand command,
    CancellationToken ct)
  {
    AuthResponse result = await command.ExecuteAsync(refreshToken, ct);
    if (result is null)
    {
      return Unauthorized("Failed to refresh token.");
    }

    return Ok(result);
  }

  [HttpPost("logout")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Logout(
    [FromBody][Required] string refreshToken,
    [FromServices] ILogoutCommand command,
    CancellationToken ct)
  {
    bool result = await command.ExecuteAsync(refreshToken, ct);
    if (!result)
    {
      return Unauthorized("Failed to logout.");
    }

    return Ok(true);
  }
}
