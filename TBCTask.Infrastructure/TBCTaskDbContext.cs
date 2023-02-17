using Microsoft.EntityFrameworkCore;
using TBCTask.Domain;

namespace TBCTask.Infrastructure;

public class TBCTaskDbContext : DbContext
{
    public TBCTaskDbContext(DbContextOptions<TBCTaskDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<RelatedPerson> RelatedPersons { get; set; }
    public DbSet<PersonPhoneNumber> PhoneNumbers { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TBCTaskDbContext).Assembly);
    }
}