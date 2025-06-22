namespace UserService.Models.Dto.Requests;

public record LogoutRequest
{
  public string RefreshToken { get; set; }
}
