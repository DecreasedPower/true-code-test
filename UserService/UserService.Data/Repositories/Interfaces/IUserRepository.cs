using UserService.Db.Models;

namespace UserService.Data.Repositories.Interfaces;

public interface IUserRepository
{
  Task<DbUser> RegisterAsync(DbUser user);
  Task<DbUser> GetAsync(string name);
}
