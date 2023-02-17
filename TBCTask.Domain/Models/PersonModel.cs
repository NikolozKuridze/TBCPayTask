using FluentValidation.Results;

namespace TBCTask.Domain.Models;

public class PersonModel
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Gender { get; set; }
    public string PrivateNumber { get; set; }
    public string BirthDate { get; set; }
    public string? City { get; set; }  
    public string? ImagePath { get; set; }
    public PersonNumbersAndRelatedPersons? Details { get; set; }
}

public class PersonNumbersAndRelatedPersons
{
    public List<RelatedPersonModel>? RelatedPersons { get; set; } 
    public List<PersonPhoneNumberModel>? PhoneNumbers { get; set; } 
}

public class AddPersonResult
{
    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public IEnumerable<ValidationFailure>? ValidationErrors { get; set; }
}