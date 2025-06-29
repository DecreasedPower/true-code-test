using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Db.Models;

[Table("RefreshTokens")]
[PrimaryKey(nameof(Id))]
public class DbRefreshToken
{
  [Key]
  public int Id { get; set; }

  [Required]
  public int UserId { get; set; }
  public DbUser User { get; set; }

  [Required]
  public string TokenHash { get; set; }

  [Required]
  public DateTime ExpiresAt { get; set; }

  [Required]
  public bool Revoked { get; set; } = false;
}
