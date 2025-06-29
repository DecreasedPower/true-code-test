using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Db.Models;

[Table("Currencies")]
[PrimaryKey(nameof(Id))]
public class DbCurrency
{
  [Key]
  public string Id { get; set; }

  [MaxLength(100)]
  [Required]
  public string Name { get; set; }

  [MaxLength(100)]
  [Required]
  public string Rate { get; set; }

  public ICollection<DbUser> Users { get; set; } = new List<DbUser>();
}
