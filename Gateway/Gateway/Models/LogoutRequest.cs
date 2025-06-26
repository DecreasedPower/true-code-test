namespace Gateway.Models;

public record LogoutRequest
{
  public string RefreshToken { get; set; }
}
