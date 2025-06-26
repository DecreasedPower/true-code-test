using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Gateway.Models;
using Gateway.Models.Configs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using LoginRequest = Gateway.Models.LoginRequest;

namespace Gateway.Controllers;

// TODO: переделать с IACTIONRESULT
// поправить свагер везде
// добавить grpc finance
// добавить контроллер тут
// пофиксить докер
[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
[Route("auth")]
public class AuthController(
  IOptions<ServiceConfiguration> serviceConfiguration)
  : Controller
{
  [HttpPost("register")]
  public async Task<IActionResult> Register(
    [FromBody][Required] LoginRequest body,
    [FromServices] IHttpClientFactory clientFactory,
    CancellationToken ct)
  {
    var response = await SendRequest(
      clientFactory.CreateClient(),
      "auth/register",
      body,
      ct);

    var result = await response.Content.ReadAsStringAsync(ct);

    return StatusCode((int)response.StatusCode, result);
  }

  [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Login(
    [FromBody][Required] LoginRequest body,
    [FromServices] IHttpClientFactory clientFactory,
    CancellationToken ct)
  {
    var response = await SendRequest(
      clientFactory.CreateClient(),
      "auth/login",
      body,
      ct);

    return StatusCode((int)response.StatusCode, await response.Content.ReadFromJsonAsync(typeof(AuthResponse), ct));
  }

  [HttpPost("refresh")]
  public async Task<IActionResult> Refresh(
    [FromBody][Required] RefreshRequest body,
    [FromServices] IHttpClientFactory clientFactory,
    CancellationToken ct)
  {
    var response = await SendRequest(
      clientFactory.CreateClient(),
      "auth/refresh",
      body,
      ct);

    var result = await response.Content.ReadAsStringAsync(ct);

    return StatusCode((int)response.StatusCode, result);
  }

  [HttpPost("logout")]
  public async Task<IActionResult> Logout(
    [FromBody][Required] LogoutRequest body,
    [FromServices] IHttpClientFactory clientFactory,
    CancellationToken ct)
  {
    var response = await SendRequest(
      clientFactory.CreateClient(),
      "auth/logout",
      body,
      ct);

    var result = await response.Content.ReadAsStringAsync(ct);

    return StatusCode((int)response.StatusCode, result);
  }

  private async Task<HttpResponseMessage> SendRequest(HttpClient client, string uri, object body, CancellationToken ct)
  {
    var json = JsonSerializer.Serialize(body);
    HttpResponseMessage response;
    using (client)
    {
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      response = await client.PostAsync(
        $"{serviceConfiguration.Value.UserService}/{uri}",
        content,
        ct);
    }

    return response;
  }
}
