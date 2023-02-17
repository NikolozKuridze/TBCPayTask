using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NLog;
using TBCTask.Domain.Interfaces;
using ILogger = NLog.ILogger;

namespace TBCTask.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly TBCTaskDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public BaseRepository(TBCTaskDbContext context, ILogger logger)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    } 

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<bool> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while adding the entity to the database");
            return false;
        }
    }

    public async Task<bool> UpdateAsync(T entity, int ID)
    {
        try
        {
            var record = GetByIdAsync(ID);
            if (record != null)
            { 
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                _logger.Info($"{entity} Updated");
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "UpdateAsync Method");
            return false;
        }

        _logger.Info($"{entity} Is Not exist,UpdateAsync Method");
        return false;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = GetByIdAsync(id).Result;
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "DeleteAsync Method");
            return false;
        }

        _logger.Info($"Entity Is Not exist ,DeleteAsync Method");
        return false;
    }
}