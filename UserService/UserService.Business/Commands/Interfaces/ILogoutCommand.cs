using UserService.Models.Dto.Requests;

namespace UserService.Business.Commands.Interfaces;

public interface ILogoutCommand
{
  Task<bool> ExecuteAsync(string refreshToken, CancellationToken ct);
}
