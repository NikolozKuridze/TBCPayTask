using System.Linq.Expressions;
using TBCTask.Domain.Models;

namespace TBCTask.Domain.Interfaces.IServices;

public interface IPersonService
{ 
    Task<PersonModel> GetPersonById(int id);
    Task<IEnumerable<PersonModel>> GetAll();
    Task<List<PersonModel>> PagedSearchPersons(int pageIndex, int pageSize, string searchPattern);
    Task<List<PersonModel>> FastSearch(string searchPattern);
    Task<AddPersonResult> AddPerson(PersonModel entity);
    Task<bool> Update(PersonModel entity);
    Task<bool> DeletePerson(int id);
    Task<bool> UpdateImage(string ImagePath, int PersonID);
}