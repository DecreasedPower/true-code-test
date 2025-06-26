using Microsoft.EntityFrameworkCore;
using UserService.Data.Repositories.Interfaces;
using UserService.Db;
using UserService.Db.Models;

namespace UserService.Data.Repositories;

public class UserRepository(
  UserServiceDbContext dbContext)
  : IUserRepository
{
  public async Task<DbUser> RegisterAsync(DbUser user)
  {
    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync();

    return await dbContext.Users.SingleAsync(u => u.Name == user.Name);
  }

  public Task<DbUser> GetAsync(string name)
  {
    return dbContext.Users.SingleAsync(u => u.Name == name);
  }
}
