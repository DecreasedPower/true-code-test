using Microsoft.EntityFrameworkCore;
using UserService.Data.Repositories;
using UserService.Data.Repositories.Interfaces;
using UserService.Db;
using UserService.Db.Models;

namespace UserService.Data.Tests;

[TestFixture]
public class RefreshTokenTests
{
  private DbContextOptions<UserServiceDbContext> _dbOptions;
  private UserServiceDbContext _context;
  private IRefreshTokenRepository _repository;

  [SetUp]
  public void Setup()
  {
    _dbOptions = new DbContextOptionsBuilder<UserServiceDbContext>()
      .UseInMemoryDatabase(databaseName: "TestDB")
      .Options;

    _context = new UserServiceDbContext(_dbOptions);

    _repository = new RefreshTokenRepository(_context);
  }

  [TearDown]
  public void TearDown()
  {
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }

  [Test]
  public async Task SuccessfullyAddsToken()
  {
    bool created = await _repository.AddAsync(
      new() { Id = 1, Revoked = false, UserId = 1, ExpiresAt = DateTime.Now.AddDays(1), TokenHash = "Token1" });

    Assert.That(created, Is.True);
  }

  [Test]
  public async Task SuccessfullyChecksToken()
  {
    SeedData();

    var token = await _repository.CheckAsync("Token1");

    Assert.That(token, Is.Not.Null);
  }

  [Test]
  public async Task NoTokenReturnsNull()
  {
    SeedData();

    var token = await _repository.CheckAsync("Token4");

    Assert.That(token, Is.Null);
  }

  [Test]
  public async Task OutdatedTokenReturnsNull()
  {
    SeedData();

    var token = await _repository.CheckAsync("Token3");

    Assert.That(token, Is.Null);
  }

  [Test]
  public async Task RevokedTokenReturnsNull()
  {
    SeedData();

    var token = await _repository.CheckAsync("Token2");

    Assert.That(token, Is.Null);
  }

  private void SeedData()
  {
    _context.Users.Add(new() { Id = 1, Name = "User1", Password = "Password1" });
    _context.RefreshTokens.AddRange(new List<DbRefreshToken>
    {
      new()
      {
        Id = 1,
        Revoked = false,
        UserId = 1,
        ExpiresAt = DateTime.Now.AddDays(1),
        TokenHash = "Token1"
      },
      new()
      {
        Id = 2,
        Revoked = true,
        UserId = 1,
        ExpiresAt = DateTime.Now.AddDays(1),
        TokenHash = "Token2"
      },
      new()
      {
        Id = 3,
        Revoked = false,
        UserId = 1,
        ExpiresAt = DateTime.Now.AddDays(-1),
        TokenHash = "Token3"
      }
    });

    _context.SaveChanges();
  }
}
