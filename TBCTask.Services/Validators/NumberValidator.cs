using FluentValidation;
using TBCTask.Domain;
using TBCTask.Domain.Models;

namespace TBCTask.Services.Validators;

public class NumberValidator: AbstractValidator<PersonPhoneNumberModel>
{
    public NumberValidator()
    {
        RuleFor(x => x.Number)
            .MinimumLength(4).WithMessage("Number Minimum Length is 4")
            .MaximumLength(50).WithMessage("Number Maximum Length is 50");
        RuleFor(x => x.NumberType)
            .NotEmpty().WithMessage("Type Is Required")
            .Must(IsValidType).WithMessage("This Type is not exist");
    }
    

    private bool IsValidType(string type)
    {
        NumberType rtype;
        if (Enum.TryParse(type, out rtype))
        {
            return true;
        }

        return false;
    }
}