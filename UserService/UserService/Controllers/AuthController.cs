using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserService.Business.Commands.Interfaces;
using UserService.Models.Dto.Requests;
using UserService.Models.Dto.Responses;
using LoginRequest = UserService.Models.Dto.Requests.LoginRequest;

namespace UserService.Controllers;

[ApiController]
[AllowAnonymous]
[Route("auth")]
public class AuthController(
  ILogger<AuthController> logger)
  : Controller
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
      logger.LogWarning("Failed to register.");
      return Unauthorized("Failed to register.");
    }

    logger.LogInformation("User {userName} successfully registered.", request.Name);
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
      logger.LogWarning("User {userName} failed to login.", request.Name);
      return Unauthorized("Failed to login.");
    }

    logger.LogInformation("User {userName} successfully logged in.", request.Name);
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
      logger.LogWarning("Failed to refresh token.");
      return Unauthorized("Failed to refresh token.");
    }

    logger.LogInformation("Refresh token success.");
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
      logger.LogWarning("Failed to logout.");
      return Unauthorized("Failed to logout.");
    }

    logger.LogInformation("Logout success.");
    return Ok(true);
  }
}
