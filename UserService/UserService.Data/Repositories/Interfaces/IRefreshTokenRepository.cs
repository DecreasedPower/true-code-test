using UserService.Db.Models;

namespace UserService.Data.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
  Task<bool> RevokeAsync(string hashedToken, CancellationToken ct = default);
  Task<bool> AddAsync(DbRefreshToken token, CancellationToken ct = default);
  Task<DbRefreshToken> CheckAsync(string tokenHash, CancellationToken ct = default);
}
