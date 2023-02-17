using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using FluentValidation;
using Microsoft.Extensions.Localization;
using NLog;
using TBCTask.Domain;
using TBCTask.Domain.Interfaces;
using TBCTask.Domain.Interfaces.IServices;
using TBCTask.Domain.Models;

namespace TBCTask.Services;

public class PersonService : IPersonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly IValidator<PersonModel> _pvalidator;
    private readonly IValidator<PersonPhoneNumberModel> _nvalidator;
    private readonly IStringLocalizer<PersonService> _localizer;


    public PersonService(IUnitOfWork unitOfWork, IValidator<PersonModel> pvalidator,
        IValidator<PersonPhoneNumberModel> nvalidator, IStringLocalizer<PersonService> localizer)
    {
        _unitOfWork = unitOfWork;
        _pvalidator = pvalidator;
        _nvalidator = nvalidator;
        _localizer = localizer;
    }

    #region Private Methods

    private PersonNumbersAndRelatedPersons GetDetails(int ID)
    {
        var details = new PersonNumbersAndRelatedPersons();
        details.PhoneNumbers = _unitOfWork.Numbers.FindAsync(x => x.PersonID == ID).Result.Select(x =>
            new PersonPhoneNumberModel
            {
                Number = x.Number,
                NumberType = Enum.GetName(typeof(NumberType), x.Type),
                PersonID = x.PersonID,
                ID = x.ID
            }).ToList();

        details.RelatedPersons = _unitOfWork.RelatedPersons.FindAsync(x => x.PersonID == ID).Result
            .Select(x => new RelatedPersonModel
            {
                ID = x.ID,
                RelatedType = Enum.GetName(typeof(RelateType), x.RelatedType),
                PersonID = x.PersonID,
                RelatedPersonID = x.RelatedPersonID
            })
            .ToList();
        return details;
    }

    private PersonModel PersonToModel(Person person)
    {
        return new PersonModel
        {
            ID = person.ID,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Gender = Enum.GetName(typeof(Gender), person.Gender),
            BirthDate = person.BirthDate.ToString("MM/dd/yyyy"),
            ImagePath = person.ImagePath,
            PrivateNumber = person.PrivateNumber
        };
    }

    public Person ModelToPerson(PersonModel model)
    {
        return new Person
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Gender = Enum.Parse<Gender>(model.Gender),
            BirthDate = Convert.ToDateTime(model.BirthDate),
            CityID = Convert.ToInt16(model.City),
            PrivateNumber = model.PrivateNumber
        };
    }

    private PersonPhoneNumberModel PersonPhoneNumberToModel(PersonPhoneNumber personPhoneNumber)
    {
        return new PersonPhoneNumberModel
        {
            Number = personPhoneNumber.Number, ID = personPhoneNumber.ID, PersonID = personPhoneNumber.PersonID
        };
    }

    #endregion
 
    public async Task<PersonModel> GetPersonById(int id)
    {
        var model = new PersonModel();
        var person = _unitOfWork.Persons.GetByIdAsync(id).Result;
        if (person.ID != 0)
        {
            model = PersonToModel(person);
            var personCity = await _unitOfWork.Cities.FindAsync(x => x.ID == person.CityID);
            if (personCity.Count() != 0)
            {
                model.City = personCity.FirstOrDefault().Name;
            }

            model.Details = GetDetails(person.ID);
        }

        return model;
    }

    public async Task<IEnumerable<PersonModel>> GetAll()
    {
        var plist = _unitOfWork.Persons.GetAllAsync().Result.ToList();
        var list = new List<PersonModel>();
        foreach (var p in plist)
        {
            var model = PersonToModel(p);
            var PersonCity = await _unitOfWork.Cities.FindAsync(x => x.ID == p.CityID);
            if (PersonCity.Count() != 0)
            {
                model.City = PersonCity.FirstOrDefault().Name;
            }

            model.Details = GetDetails(p.ID);
            list.Add(model);
        }

        return list;
    }

    public async Task<List<PersonModel>> PagedSearchPersons(int pageIndex, int pageSize, string searchPattern)
    {
        var plist = _unitOfWork.Persons.PagedSearchPersons(pageIndex, pageSize, searchPattern).Result.ToList();
        var list = new List<PersonModel>();
        foreach (var p in plist)
        {
            var model = PersonToModel(p);
            var PersonCity = await _unitOfWork.Cities.FindAsync(x => x.ID == p.CityID);
            if (PersonCity.Count() != 0)
            {
                model.City = PersonCity.FirstOrDefault().Name;
            }

            model.Details = GetDetails(p.ID);
            list.Add(model);
        }

        return list;
    }

    public async Task<List<PersonModel>> FastSearch(string searchPattern)
    {
        var plist = _unitOfWork.Persons.FastSearchPersons(searchPattern).Result.ToList();
        var list = new List<PersonModel>();
        foreach (var p in plist)
        {
            var model = PersonToModel(p);
            var PersonCity = await _unitOfWork.Cities.FindAsync(x => x.ID == p.CityID);
            if (PersonCity.Count() != 0)
            {
                model.City = PersonCity.FirstOrDefault().Name;
            }

            model.Details = GetDetails(p.ID);
            list.Add(model);
        }

        return list;
    }


    public async Task<AddPersonResult> AddPerson(PersonModel entity)
    {
        try
        {
            var isValid = _pvalidator.Validate(entity);
            if (isValid.IsValid)
            {
                _logger.Info("Creating Person");
                var IsCreatedUser = await _unitOfWork.Persons.AddAsync(ModelToPerson(entity));
                if (IsCreatedUser)
                {
                    if (entity.Details.PhoneNumbers != null)
                    {
                        var thisperson =
                            _unitOfWork.Persons.FindAsync(x => x.PrivateNumber == entity.PrivateNumber);
                        entity.ID = thisperson.Result.FirstOrDefault()!.ID;
                        foreach (var num in entity.Details.PhoneNumbers)
                        {
                            var IsValidNumber = _nvalidator.Validate(num);
                            if (IsValidNumber.IsValid)
                            {
                                var n = new PersonPhoneNumber
                                {
                                    Number = num.Number, PersonID = entity.ID,
                                    Type = (NumberType)Enum.Parse(typeof(NumberType), num.NumberType)
                                };
                                var adn = await _unitOfWork.Numbers.AddAsync(n);
                                if (!adn)
                                {
                                    throw new Exception();
                                }

                                return new AddPersonResult { IsSuccessful = true };
                            }

                            return new AddPersonResult
                                { IsSuccessful = false, ValidationErrors = IsValidNumber.Errors };
                        }
                    }
                }
                else
                {
                    return new AddPersonResult { IsSuccessful = false };
                }
            }

            return new AddPersonResult { IsSuccessful = false, ValidationErrors = isValid.Errors };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Add Person Method");
            return new AddPersonResult { IsSuccessful = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<bool> Update(PersonModel entity)
    {
        var person = ModelToPerson(entity);
        var IsUpdated = await _unitOfWork.Persons.UpdateAsync(person, person.ID);
        return IsUpdated;
    }

    public async Task<bool> DeletePerson(int id)
    {
        return await _unitOfWork.Persons.DeletePersonAsync(id);
    }

    public async Task<bool> UpdateImage(string ImagePath, int PersonID)
    {
        var IsUpdated = await _unitOfWork.Persons.UpdateImageAsync(ImagePath, PersonID);
        return IsUpdated;
    }
}