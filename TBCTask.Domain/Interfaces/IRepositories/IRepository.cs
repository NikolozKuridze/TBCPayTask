using System.Linq.Expressions;

namespace TBCTask.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity, int ID);
    Task<bool> DeleteAsync(int Id);
}