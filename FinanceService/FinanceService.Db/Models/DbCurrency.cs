using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Db.Models;

[Table("Currencies")]
[PrimaryKey(nameof(Id))]
public class DbCurrency
{
  [Key]
  public string Id { get; set; }

  [MaxLength(100)]
  public string Name { get; set; }

  [MaxLength(100)]
  public string Rate { get; set; }

  public ICollection<DbUserCurrency> UsersCurrencies { get; set; }
}
