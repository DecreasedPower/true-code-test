using Microsoft.EntityFrameworkCore;
using UserService.Data.Repositories;
using UserService.Data.Repositories.Interfaces;
using UserService.Db;
using UserService.Db.Models;

namespace UserService.Data.Tests;

[TestFixture]
public class UserRepositoryTests
{
  private DbContextOptions<UserServiceDbContext> _dbOptions;
  private UserServiceDbContext _context;
  private IUserRepository _repository;

  [SetUp]
  public void Setup()
  {
    _dbOptions = new DbContextOptionsBuilder<UserServiceDbContext>()
      .UseInMemoryDatabase(databaseName: "TestDB")
      .Options;

    _context = new UserServiceDbContext(_dbOptions);

    _repository = new UserRepository(_context);
  }

  [TearDown]
  public void TearDown()
  {
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }

  [Test]
  public async Task SuccessfullyRegistersUser()
  {
    SeedData();

    var createdUser = await _repository.RegisterAsync(
      new DbUser { Id = 3, Name = "User3", Password = "Password3" });

    Assert.That(createdUser, Is.Not.Null);
  }

  [Test]
  public void FailsRegisterOccupiedName()
  {
    SeedData();

    Assert.ThrowsAsync<InvalidOperationException>(async () =>
    {
      await _repository.RegisterAsync(
        new DbUser { Id = 3, Name = "User1", Password = "Password3" });
    });
  }

  [Test]
  public async Task SuccessfullyReturnsRegisteredUser()
  {
    SeedData();

    var user = await _repository.GetAsync("User1");

    Assert.That(user, Is.Not.Null);
  }

  [Test]
  public void GetNotExistingThrowsException()
  {
    SeedData();

    Assert.ThrowsAsync<InvalidOperationException>(async () =>
    {
      await _repository.GetAsync("User3");
    });
  }

  private void SeedData()
  {
    _context.Users.AddRange(new List<DbUser>
    {
      new() { Id = 1, Name = "User1", Password = "Password1"},
      new() { Id = 2, Name = "User2", Password = "Password2"}
    });

    _context.SaveChanges();
  }
}
