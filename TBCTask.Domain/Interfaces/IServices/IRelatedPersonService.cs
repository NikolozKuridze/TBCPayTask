using TBCTask.Domain.Models;

namespace TBCTask.Domain.Interfaces.IServices;

public interface IRelatedPersonService
{
    Task<AddPersonResult> AddRelatedPerson(RelatedPersonModel entity);
    Task<bool> DeleteRelatedPerson(int id);
}