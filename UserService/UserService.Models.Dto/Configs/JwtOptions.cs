using System.Security.Cryptography;

namespace UserService.Models.Dto.Configs;

public class JwtOptions
{
  public string Issuer { get; set; } = string.Empty;
  public string Audience { get; set; } = string.Empty;
  public int AccessTokenExpiryMinutes { get; set; }
  public int RefreshTokenExpiryMinutes { get; set; }

  public string PrivateKeyBase64 { get; set; }
  public RSA PrivateKey { get; set; }

  public string PublicKeyBase64 { get; set; }
  public RSA PublicKey { get; set; }
}
