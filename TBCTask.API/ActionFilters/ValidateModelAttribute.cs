namespace TBCTask.API.ActionFilters;

using System;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(errors);
            return;
        }

        var validator = (IValidator)context.HttpContext.RequestServices.GetService(
            typeof(IValidator<>).MakeGenericType(context.ActionDescriptor.Parameters.First().ParameterType));

        if (validator == null)
        {
            throw new InvalidOperationException(
                $"Validator not found for {context.ActionDescriptor.Parameters.First().ParameterType.Name}");
        }

        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value != null)
            {
                var validationResult = validator.Validate(new ValidationContext<object>(argument.Value));

                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                    return;
                }
            }
        }
    }
}