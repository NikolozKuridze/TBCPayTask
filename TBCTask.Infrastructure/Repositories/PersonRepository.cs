using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NLog;
using TBCTask.Domain;
using TBCTask.Domain.Interfaces;
using ILogger = NLog.ILogger;


namespace TBCTask.Infrastructure.Repositories;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(TBCTaskDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public async Task<bool> IsExistPerson(int id)
    {
        var IsExist = _context.Persons.FirstOrDefault(x => x.ID == id);
        if (IsExist != null && IsExist.ID != 0)
        {
            return true;
        }

        return false;
    }

    public Task<IEnumerable<Person>> FastSearchPersons(string searchPattern)
    {
        var persons = _context.Persons.AsQueryable();

        if (!string.IsNullOrEmpty(searchPattern))
        {
            persons = persons.Where
            (p => p.FirstName.Contains(searchPattern) ||
                  p.LastName.Contains(searchPattern) ||
                  p.PrivateNumber.Contains(searchPattern) ||
                  p.BirthDate.ToString().Contains(searchPattern)
            );
        }

        return Task.FromResult<IEnumerable<Person>>(persons.ToList());
    }

    public Task<IEnumerable<Person>> PagedSearchPersons(int pageIndex, int pageSize, string searchPattern)
    {
        var persons = _context.Persons.AsQueryable();

        if (!string.IsNullOrEmpty(searchPattern))
        {
            persons = persons.Where
            (p => p.FirstName.Contains(searchPattern) ||
                  p.LastName.Contains(searchPattern) ||
                  p.PrivateNumber.Contains(searchPattern) ||
                  p.BirthDate.ToString().Contains(searchPattern)
            );
        }

        persons = persons.OrderBy(p => p.ID)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        return Task.FromResult<IEnumerable<Person>>(persons.ToList());
    }

    public Task<bool> UpdateImageAsync(string ImagePath, int ID)
    {
        var person = _context.Persons.FirstOrDefault(x => x.ID == ID);
        if (person != null)
        {
            person.ImagePath = ImagePath;
            _context.Persons.Update(person);
            _context.SaveChangesAsync();
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public async Task<bool> DeletePersonAsync(int id)
    {
        var person = _context.Persons.FirstOrDefault(x => x.ID == id);
        if (person != null)
        {
            _context.Persons.Remove(person);
            var numbers = _context.PhoneNumbers.Where(x => x.PersonID == id).ToList();
            if (numbers.Count > 0)
            {
                foreach (var n in numbers)
                {
                    _context.PhoneNumbers.Remove(n);
                }
            }

            return true;
        }

        return false;
    }
}