namespace TBCTask.Domain.Interfaces;

public interface IPersonRepository : IRepository<Person>
{
    Task<bool> UpdateImageAsync(string ImagePath, int ID);
    Task<bool> DeletePersonAsync(int id);
    Task<bool> IsExistPerson(int id);
    Task<IEnumerable<Person>> FastSearchPersons(string searchPattern);
    Task<IEnumerable<Person>> PagedSearchPersons(int pageIndex, int pageSize, string searchPattern);
}