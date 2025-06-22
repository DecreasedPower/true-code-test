using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserService.Db.Models;
using UserService.Models.Dto.Configs;

namespace UserService.Business.Helpers;

public static class TokenHelper
{
  public static string GenerateAccessToken(DbUser user, JwtOptions options)
  {
    var creds = new SigningCredentials(
      new RsaSecurityKey(options.PrivateKey),
      SecurityAlgorithms.RsaSha256
    );

    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Name, user.Name),
    };

    var token = new JwtSecurityToken(
      issuer: options.Issuer,
      audience: options.Audience,
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(options.AccessTokenExpiryMinutes),
      signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  public static string GenerateRefreshToken()
  {
    var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    return token;
  }

  public static string ComputeHash(string token)
  {
    return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
  }
}
