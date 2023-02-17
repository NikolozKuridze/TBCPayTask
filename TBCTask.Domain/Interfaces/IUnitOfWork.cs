namespace TBCTask.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPersonRepository Persons { get; }
    ICityRepository Cities { get; }
    INumberRepository Numbers { get; }
    IRelatedPersonRepository RelatedPersons { get; }
    Task SaveChangesAsync();
}