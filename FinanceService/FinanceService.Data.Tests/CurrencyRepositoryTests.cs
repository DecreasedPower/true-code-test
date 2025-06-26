using FinanceService.Data.Repositories;
using FinanceService.Data.Repositories.Interfaces;
using FinanceService.Db;
using FinanceService.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Data.Tests;

[TestFixture]
public class CurrencyRepositoryTests
{
  private DbContextOptions<FinanceServiceDbContext> _dbOptions;
  private FinanceServiceDbContext _context;
  private ICurrencyRepository _repository;

  [SetUp]
  public void Setup()
  {
    _dbOptions = new DbContextOptionsBuilder<FinanceServiceDbContext>()
      .UseInMemoryDatabase(databaseName: "TestDB")
      .Options;

    _context = new FinanceServiceDbContext(_dbOptions);

    _repository = new CurrencyRepository(_context);
  }

  [TearDown]
  public void TearDown()
  {
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }

  [Test]
  public async Task ReturnsActualData()
  {
    _context.Set<DbCurrency>().AddRange(new List<DbCurrency>
    {
      new() { Id = "Currency1", Name = "CurrencyName1", Rate = "CurrencyRate1" },
      new() { Id = "Currency2", Name = "CurrencyName2", Rate = "CurrencyRate2" }
    });
    _context.SaveChanges();

    var currencies = await _repository.GetAvailableCurrencies();
    Assert.That(currencies, Has.Count.EqualTo(2));
  }

  [Test]
  public async Task ReturnsNoDataIfEmpty()
  {
    var currencies = await _repository.GetAvailableCurrencies();
    Assert.That(currencies, Has.Count.EqualTo(0));
  }
}
