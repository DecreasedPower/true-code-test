using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Db.Models;

[Table("RefreshTokens")]
[PrimaryKey(nameof(Id))]
public class DbRefreshToken
{
  [Key]
  public int Id { get; set; }
  public int UserId { get; set; }
  public DbUser User { get; set; }

  public string TokenHash { get; set; }
  public DateTime ExpiresAt { get; set; }
  public bool Revoked { get; set; } = false;
}
