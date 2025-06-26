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

public class RegisterCommand(
  IUserRepository userRepository,
  IRefreshTokenRepository refreshTokenRepository,
  IOptions<JwtOptions> jwtOptions,
  ILogger<RegisterCommand> logger)
  : IRegisterCommand
{
  public async Task<AuthResponse> ExecuteAsync(LoginRequest request, CancellationToken ct)
  {
    DbUser registeredUser;
    try
    {
      registeredUser = await userRepository.RegisterAsync(new DbUser
      {
        Id = 0,
        Name = request.Name,
        Password = Crypt.EnhancedHashPassword(request.Password)
      });
    }
    catch (Exception e)
    {
      logger.LogWarning(e, "Failed to register.");
      return null;
    }

    var accessToken = TokenHelper.GenerateAccessToken(registeredUser, jwtOptions.Value);
    var refreshToken = TokenHelper.GenerateRefreshToken();

    await refreshTokenRepository.AddAsync(new DbRefreshToken
    {
      Id = 0,
      UserId = registeredUser.Id,
      Revoked = true,
      ExpiresAt = DateTime.UtcNow.AddMinutes(jwtOptions.Value.RefreshTokenExpiryMinutes),
      TokenHash = refreshToken
    }, ct);

    return new AuthResponse
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken
    };
  }
}
