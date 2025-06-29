using FinanceService.Data.Repositories;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db;
using FinanceService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Data.Tests;

[TestFixture]
public class UserCurrencyRepositoryTests
{
  private DbContextOptions<FinanceServiceDbContext> _dbOptions;
  private FinanceServiceDbContext _context;
  private IUserCurrencyRepository _repository;

  private const int UserId = 1;

  [SetUp]
  public void Setup()
  {
    _dbOptions = new DbContextOptionsBuilder<FinanceServiceDbContext>()
      .UseInMemoryDatabase(databaseName: "TestDB")
      .Options;

    _context = new FinanceServiceDbContext(_dbOptions);

    _repository = new UserCurrencyRepository(_context);
  }

  [TearDown]
  public void TearDown()
  {
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }

  [Test]
  public async Task GetReturnsOnlyUserCurrencies()
  {
    SeedCurrencies();

    _context.UserCurrencies.AddRange(new List<DbUserCurrency>
    {
      new() { CurrencyId = "1", UserId = UserId },
      new() { CurrencyId = "2", UserId = 2 }
    });

    _context.SaveChanges();

    var currencies = await _repository.GetAsync(UserId);
    Assert.That(currencies, Has.Count.EqualTo(1));
  }

  [Test]
  public async Task GetReturnsEmptyList()
  {
    SeedCurrencies();

    _context.UserCurrencies.AddRange(new List<DbUserCurrency>
    {
      new() { CurrencyId = "1", UserId = UserId },
      new() { CurrencyId = "2", UserId = 2 }
    });

    _context.SaveChanges();

    var currencies = await _repository.GetAsync(3);
    Assert.That(currencies, Has.Count.EqualTo(0));
  }

  [Test]
  public async Task SuccessfullyAdds()
  {
    SeedCurrencies();

    _context.UserCurrencies.AddRange(new List<DbUserCurrency>
    {
      new() { CurrencyId = "1", UserId = UserId },
      new() { CurrencyId = "2", UserId = 2 }
    });

    _context.SaveChanges();

    await _repository.AddAsync(new DbUserCurrency { CurrencyId = "2", UserId = UserId });
    var currencies = await _repository.GetAsync(UserId);
    Assert.That(currencies, Has.Count.EqualTo(2));
  }

  [Test]
  public Task UnsuccessfulAddThrowsException()
  {
    SeedCurrencies();

    _context.UserCurrencies.AddRange(new List<DbUserCurrency>
    {
      new() { CurrencyId = "1", UserId = UserId },
      new() { CurrencyId = "2", UserId = 2 }
    });

    _context.SaveChanges();

    Assert.ThrowsAsync<InvalidOperationException>(async () =>
    {
      await _repository.AddAsync(new DbUserCurrency { CurrencyId = "1", UserId = UserId });
    });

    return Task.CompletedTask;
  }

  // I wish I could test update and delete methods
  // but In-memory DB supports very limited range of
  // operations :)

  private void SeedCurrencies()
  {
    _context.Currencies.AddRange(
      new DbCurrency { Id = "1", Name = "Currency1", Rate = "Rate1" },
      new DbCurrency { Id = "2", Name = "Currency2", Rate = "Rate2" },
      new DbCurrency { Id = "3", Name = "Currency3", Rate = "Rate3" });

    _context.SaveChanges();
  }
}
