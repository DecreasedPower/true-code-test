using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Db.Models;

[Table("Users")]
[PrimaryKey(nameof(Id))]
public class DbUser
{
  [Key]
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string Name { get; set; }

  [Required]
  [MaxLength(100)]
  public string Password { get; set; }

  public ICollection<DbCurrency> Currencies { get; set; } = new List<DbCurrency>();
}
