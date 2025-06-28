using System.Security.Cryptography;

namespace UserService.Models.Dto.Configs;

public class JwtOptions
{
  public string Issuer { get; set; } = string.Empty;
  public string Audience { get; set; } = string.Empty;
  public int AccessTokenExpiryMinutes { get; set; }
  public int RefreshTokenExpiryDays { get; set; }

  public string PrivateKeyBase64 { get; set; }
  public RSA PrivateKey { get; set; }
}
