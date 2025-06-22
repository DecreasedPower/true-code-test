using UserService.Business.Commands.Interfaces;
using UserService.Business.Helpers;
using UserService.Data.Repositories.Interfaces;
using UserService.Models.Dto.Requests;

namespace UserService.Business.Commands;

public class LogoutCommand(
  IRefreshTokenRepository refreshTokenRepository)
  : ILogoutCommand
{
  public Task<bool> ExecuteAsync(LogoutRequest request, CancellationToken ct)
  {
    var tokenHash = TokenHelper.ComputeHash(request.RefreshToken);

    return refreshTokenRepository.RevokeAsync(tokenHash, ct);
  }
}
