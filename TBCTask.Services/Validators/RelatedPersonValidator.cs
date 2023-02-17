using FluentValidation;
using TBCTask.Domain;
using TBCTask.Domain.Interfaces;
using TBCTask.Domain.Models;

namespace TBCTask.Services.Validators;

public class RelatedPersonValidator : AbstractValidator<RelatedPersonModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public RelatedPersonValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(x => x.RelatedType)
            .NotEmpty().WithMessage("Related Tyoe Is Required")
            .Must(IsValidType).WithMessage("This Relate Type Is not exist");
        RuleFor(x => x.PersonID)
            .NotEmpty().WithMessage("PersonID is Required")
            .Must(IsExist).WithMessage("Person Is Not Exist");
        RuleFor(x => x.RelatedPersonID)
            .NotEmpty().WithMessage("Related Person ID is Required")
            .Must(IsExist).WithMessage("Person Is Not Exist");
    }

    private bool IsExist(int ID)
    {
        return _unitOfWork.Persons.IsExistPerson(ID).Result;
    }

    private bool IsValidType(string type)
    {
        RelateType rtype;
        if (Enum.TryParse(type, out rtype))
        {
            return true;
        }

        return false;
    }
}