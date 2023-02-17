using Microsoft.Extensions.Logging;
using TBCTask.Domain;
using TBCTask.Domain.Interfaces;
using ILogger = NLog.ILogger;


namespace TBCTask.Infrastructure.Repositories;

public class RelatedPersonRepository : BaseRepository<RelatedPerson>, IRelatedPersonRepository
{
    public RelatedPersonRepository(TBCTaskDbContext context, ILogger logger) : base(context, logger)
    {
    } 
}