using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Business.Commands.Interfaces;
using UserService.Business.Helpers;
using UserService.Data.Repositories.Interfaces;
using UserService.Db.Models;
using UserService.Models.Dto.Configs;
using UserService.Models.Dto.Responses;

namespace UserService.Business.Commands;

public class RefreshCommand(
  IRefreshTokenRepository refreshTokenRepository,
  IOptions<JwtOptions> jwtOptions,
  ILogger<RefreshCommand> logger)
  : IRefreshCommand
{
  public async Task<AuthResponse> ExecuteAsync(string refreshToken, CancellationToken ct)
  {
    string tokenHash = TokenHelper.ComputeHash(refreshToken);
    var token = await refreshTokenRepository.CheckAsync(tokenHash, ct);
    if (token is null)
    {
      logger.LogWarning("Failed to refresh token.");
      return null;
    }

    await Task.Run(() => refreshTokenRepository.RevokeAsync(tokenHash, ct), ct);
    var newRefreshToken = TokenHelper.GenerateRefreshToken();
    await refreshTokenRepository.AddAsync(new DbRefreshToken
    {
      UserId = token.UserId,
      ExpiresAt = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpiryDays),
      TokenHash = TokenHelper.ComputeHash(newRefreshToken)
    }, ct);

    return new AuthResponse
    {
      AccessToken = TokenHelper.GenerateAccessToken(token.User, jwtOptions.Value),
      RefreshToken = newRefreshToken
    };
  }
}
