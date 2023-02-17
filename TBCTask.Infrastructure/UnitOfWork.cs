using Microsoft.Extensions.Logging;
using NLog;
using TBCTask.Domain;
using TBCTask.Domain.Interfaces;
using TBCTask.Infrastructure.Repositories;
using ILogger = NLog.ILogger;

namespace TBCTask.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public IPersonRepository Persons { get; private set; }
    public ICityRepository Cities { get; private set; }
    public INumberRepository Numbers { get; private set; }
    public IRelatedPersonRepository RelatedPersons { get; private set; }
    private readonly TBCTaskDbContext _context;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();


    public UnitOfWork(TBCTaskDbContext context)
    {
        _context = context;

        Persons = new PersonRepository(_context, _logger);
        Numbers = new NumberRepository(_context, _logger);
        Cities = new CityRepository(_context, _logger);
        RelatedPersons = new RelatedPersonRepository(_context, _logger);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
        _logger.Info("SaveChangesAsync");
    }
}