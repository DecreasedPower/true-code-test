using UserService.Models.Dto.Requests;

namespace UserService.Business.Commands.Interfaces;

public interface ILogoutCommand
{
  Task<bool> ExecuteAsync(LogoutRequest request, CancellationToken ct);
}
