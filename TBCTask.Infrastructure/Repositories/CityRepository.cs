using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NLog;
using TBCTask.Domain;
using TBCTask.Domain.Interfaces;
using ILogger = NLog.ILogger;


namespace TBCTask.Infrastructure.Repositories;

public class CityRepository : BaseRepository<City>, ICityRepository
{
    public CityRepository(TBCTaskDbContext context, ILogger logger) : base(context, logger)
    {
    }
}