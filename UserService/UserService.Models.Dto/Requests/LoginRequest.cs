using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dto.Requests;

public record LoginRequest
{
  [Required]
  [MaxLength(100)]
  public string Name { get; set; }

  [Required]
  [MaxLength(100)]
  public string Password { get; set; }
}
