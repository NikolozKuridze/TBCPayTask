using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Localization;
using TBCTask.Domain;
using TBCTask.Domain.Models;

namespace TBCTask.Services.Validators;

public class PersonValidator : AbstractValidator<PersonModel>
{
    private readonly IStringLocalizer<PersonValidator> _localizer;

    public PersonValidator(IStringLocalizer<PersonValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(_localizer["emptyfn"].Value)
            .Must(IsValidLanguage).WithMessage("First name must contain only Georgian or American characters")
            .MinimumLength(2).WithMessage("Minimum Length is 2")
            .MaximumLength(50).WithMessage("Maximum Length is 50");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please Enter Your Last Name")
            .Must(IsValidLanguage).WithMessage("Last name must contain only Georgian or American characters")
            .MinimumLength(2).WithMessage("Minimum Length is 2")
            .MaximumLength(50).WithMessage("Maximum Length is 50");

        RuleFor(p => p.PrivateNumber)
            .NotEmpty().WithMessage("Private number is required")
            .Length(11).WithMessage("Private number must be 11 digits")
            .Matches("^[0-9]*$").WithMessage("Private number must contain only digits");

        RuleFor(x => Convert.ToDateTime(x.BirthDate))
            .NotEmpty().WithMessage("Birthdate Is Required")
            .Must(IsValidAge).WithMessage("Sorry, You are less than 18");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("please Select your gender")
            .Must(IsValidGenderType).WithMessage("this Gender not exist");
    }

    private bool IsValidAge(DateTime birthdate)
    {
        return DateTime.Now.Date.Year - birthdate.Date.Year >= 18;
    }

    private bool IsValidLanguage(string input)
    {
        string georgianChars = "\u10D0-\u10F0"; // Georgian Unicode range
        string americanChars = "a-zA-Z"; // English alphabet range 

        string georgianPattern = $"^[{georgianChars}]+$";
        string americanPattern = $"^[{americanChars}]+$";

        bool isGeorgian = Regex.IsMatch(input, georgianPattern);
        bool isAmerican = Regex.IsMatch(input, americanPattern);

        return isGeorgian || isAmerican;
    }

    private bool IsValidGenderType(string type)
    {
        NumberType gtype;
        if (Enum.TryParse(type, out gtype))
        {
            return true;
        }

        return false;
    }
}