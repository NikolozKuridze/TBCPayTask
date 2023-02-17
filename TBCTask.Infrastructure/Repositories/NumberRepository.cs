using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NLog;
using TBCTask.Domain;
using TBCTask.Domain.Interfaces;
using ILogger = NLog.ILogger;


namespace TBCTask.Infrastructure.Repositories;

public class NumberRepository : BaseRepository<PersonPhoneNumber>, INumberRepository
{
    public NumberRepository(TBCTaskDbContext context, ILogger logger) : base(context, logger)
    {
    }
}