using System.ComponentModel.DataAnnotations;

namespace Gateway.Models;

public record AuthResponse
{
  [Required]
  public string AccessToken { get; set; }

  [Required]
  public string RefreshToken { get; set; }
}
