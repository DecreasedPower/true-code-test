using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserService.Business.Commands.Interfaces;
using UserService.Models.Dto.Requests;
using UserService.Models.Dto.Responses;
using LoginRequest = UserService.Models.Dto.Requests.LoginRequest;

namespace UserService.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : Controller
{
  [HttpPost("register")]
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
  public async Task<IActionResult> Refresh(
    [FromBody][Required] RefreshRequest request,
    [FromServices] IRefreshCommand command,
    CancellationToken ct)
  {
    AuthResponse result = await command.ExecuteAsync(request, ct);
    if (result is null)
    {
      return Unauthorized("Failed to refresh token.");
    }

    return Ok(result);
  }

  [HttpPost("logout")]
  public async Task<IActionResult> Logout(
    [FromBody][Required] LogoutRequest request,
    [FromServices] ILogoutCommand command,
    CancellationToken ct)
  {
    bool result = await command.ExecuteAsync(request, ct);
    if (!result)
    {
      return Unauthorized("Failed to logout.");
    }

    return Ok(true);
  }
}
