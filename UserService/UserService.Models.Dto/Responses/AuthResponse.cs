namespace UserService.Models.Dto.Responses;

public record AuthResponse
{
  public string AccessToken { get; set; }
  public string RefreshToken { get; set; }
}
