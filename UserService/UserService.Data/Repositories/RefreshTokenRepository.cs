using Microsoft.EntityFrameworkCore;
using UserService.Data.Repositories.Interfaces;
using UserService.Db;
using UserService.Db.Models;

namespace UserService.Data.Repositories;

public class RefreshTokenRepository(
  UserServiceDbContext dbContext)
  : IRefreshTokenRepository
{
  public async Task<bool> RevokeAsync(string hashedToken, CancellationToken ct = default)
  {
    int updatedRows = await dbContext.RefreshTokens
      .Where(rt => !rt.Revoked && rt.TokenHash == hashedToken)
      .ExecuteUpdateAsync(
        u => u.SetProperty(r => r.Revoked, true),
        ct);

    return updatedRows != 0;
  }

  public async Task<bool> AddAsync(DbRefreshToken token, CancellationToken ct = default)
  {
    dbContext.RefreshTokens.Add(token);

    await dbContext.SaveChangesAsync(ct);
    return true;
  }

  public async Task<DbRefreshToken> CheckAsync(string tokenHash, CancellationToken ct = default)
  {
    return await dbContext.RefreshTokens
      .Include(rt => rt.User)
      .FirstOrDefaultAsync(rt =>
        !rt.Revoked &&
        rt.TokenHash == tokenHash &&
        rt.ExpiresAt > DateTime.Now, ct);
  }
}
