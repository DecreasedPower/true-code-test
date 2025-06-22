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
    try
    {
      dbContext.Users.Add(user);
      await dbContext.SaveChangesAsync();
    }
    catch (Exception e)
    {
      return null;
    }

    return await dbContext.Users.SingleAsync(u => u.Name == user.Name);
  }

  public Task<DbUser> GetAsync(string name)
  {
    return dbContext.Users.FirstOrDefaultAsync(u => u.Name == name);
  }
}
