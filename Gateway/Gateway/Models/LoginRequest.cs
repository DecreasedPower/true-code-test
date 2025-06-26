using System.ComponentModel.DataAnnotations;

namespace Gateway.Models;

public record LoginRequest
{
  [Required]
  [MaxLength(100)]
  public string Name { get; set; }

  [Required]
  [MaxLength(100)]
  public string Password { get; set; }
}
