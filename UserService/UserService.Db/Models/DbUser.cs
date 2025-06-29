using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Db.Models;

[Table("Users")]
[PrimaryKey(nameof(Id))]
public class DbUser
{
  [Key]
  public int Id { get; set; }

  [MaxLength(100)]
  public string Name { get; set; }

  [MaxLength(100)]
  public string Password { get; set; }
}
