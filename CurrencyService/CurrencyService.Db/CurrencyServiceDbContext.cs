using CurrencyService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Db;

public class CurrencyServiceDbContext(DbContextOptions<CurrencyServiceDbContext> options) : DbContext(options)
{
  public DbSet<DbCurrency> Currencies { get; set; }
  public DbSet<DbUser> Users { get; set; }
  public DbSet<DbRefreshToken> RefreshTokens { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<DbUser>()
      .HasIndex(u => u.Name)
      .IsUnique();

    modelBuilder.Entity<DbUser>()
      .HasMany(u => u.Currencies)
      .WithMany(c => c.Users)
      .UsingEntity<Dictionary<string, object>>(
        "UsersCurrencies",
        j => j
          .HasOne<DbCurrency>()
          .WithMany()
          .HasForeignKey("CurrencyId")
          .HasConstraintName("FK_UsersCurrencies_CurrencyId")
          .OnDelete(DeleteBehavior.Cascade),
        j => j
          .HasOne<DbUser>()
          .WithMany()
          .HasForeignKey("UserId")
          .HasConstraintName("FK_UsersCurrencies_UserId")
          .OnDelete(DeleteBehavior.Cascade),
        j =>
        {
          j.HasKey("UserId", "CurrencyId");
          j.ToTable("UsersCurrencies");
          j.IndexerProperty<int>("UserId").HasColumnName("UserId");
          j.IndexerProperty<string>("CurrencyId").HasColumnName("CurrencyId");
        });

    modelBuilder.Entity<DbRefreshToken>()
      .HasOne(r => r.User);
  }
}
