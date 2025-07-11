using Microsoft.AspNetCore.Identity.Data;
using UserService.Models.Dto.Responses;

namespace UserService.Business.Commands.Interfaces;

public interface IRefreshCommand
{
  Task<AuthResponse> ExecuteAsync(string refreshToken, CancellationToken ct);
}
