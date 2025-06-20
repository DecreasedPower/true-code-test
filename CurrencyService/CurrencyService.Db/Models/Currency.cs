using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Db.Models;

[PrimaryKey(nameof(Id))]
public class Currency
{
  public int Id { get; set; }

  [MaxLength(100)]
  public string Name { get; set; }

  public decimal Rate { get; set; }
}
