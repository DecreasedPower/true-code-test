using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Db.Models;

[Table("UsersCurrencies")]
[PrimaryKey(nameof(UserId), nameof(CurrencyId))]
public class DbUserCurrency
{
  public int UserId { get; set; }

  [MaxLength(100)]
  public string CurrencyId { get; set; }
}
