using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Business.Commands.Interfaces;
using UserService.Business.Helpers;
using UserService.Data.Repositories.Interfaces;
using UserService.Db.Models;
using UserService.Models.Dto.Configs;
using UserService.Models.Dto.Requests;
using UserService.Models.Dto.Responses;
using Crypt = BCrypt.Net.BCrypt;

namespace UserService.Business.Commands;

public class LoginCommand(
  IUserRepository userRepository,
  IRefreshTokenRepository refreshTokenRepository,
  IOptions<JwtOptions> jwtOptions,
  ILogger<LoginCommand> logger)
  : ILoginCommand
{
  public async Task<AuthResponse> ExecuteAsync(LoginRequest request, CancellationToken ct)
  {
    DbUser user;
    try
    {
      user = await userRepository.GetAsync(request.Name);
    }
    catch (Exception e)
    {
      logger.LogWarning(e, "Failed to login.");
      return null;
    }

    if (!Crypt.EnhancedVerify(request.Password, user.Password))
    {
      return null;
    }

    var accessToken = TokenHelper.GenerateAccessToken(user, jwtOptions.Value);
    var refreshToken = TokenHelper.GenerateRefreshToken();

    await refreshTokenRepository.AddAsync(new DbRefreshToken
    {
      Id = 0,
      UserId = user.Id,
      Revoked = false,
      ExpiresAt = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpiryDays),
      TokenHash = TokenHelper.ComputeHash(refreshToken)
    }, ct);

    return new AuthResponse
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken
    };
  }
}
