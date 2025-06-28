using System.ComponentModel.DataAnnotations;

namespace Gateway.Models;

public record LogoutRequest
{
  [Required]
  public string RefreshToken { get; set; }
}
