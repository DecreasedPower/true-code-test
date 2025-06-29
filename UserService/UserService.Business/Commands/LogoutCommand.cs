using UserService.Business.Commands.Interfaces;
using UserService.Business.Helpers;
using UserService.Data.Repositories.Interfaces;

namespace UserService.Business.Commands;

public class LogoutCommand(
  IRefreshTokenRepository refreshTokenRepository)
  : ILogoutCommand
{
  public Task<bool> ExecuteAsync(string refreshToken, CancellationToken ct)
  {
    string tokenHash = TokenHelper.ComputeHash(refreshToken);
    return refreshTokenRepository.RevokeAsync(tokenHash, ct);
  }
}
