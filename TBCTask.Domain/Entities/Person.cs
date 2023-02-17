using System.ComponentModel.DataAnnotations;

namespace TBCTask.Domain;

public class Person : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public string PrivateNumber { get; set; }
    [DataType(DataType.Date)] public DateTime BirthDate { get; set; }
    public int CityID { get; set; }
    public string? ImagePath { get; set; }
    public virtual ICollection<PersonPhoneNumber> PhoneNumbers { get; set; }
    public virtual ICollection<RelatedPerson> RelatedPersons { get; set; }
}