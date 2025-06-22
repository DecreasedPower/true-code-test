using UserService.Models.Dto.Requests;
using UserService.Models.Dto.Responses;

namespace UserService.Business.Commands.Interfaces;

public interface IRegisterCommand
{
  Task<AuthResponse> ExecuteAsync(LoginRequest request, CancellationToken ct);
}
