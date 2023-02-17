using FluentValidation;
using NLog;
using TBCTask.Domain;
using TBCTask.Domain.Interfaces;
using TBCTask.Domain.Interfaces.IServices;
using TBCTask.Domain.Models;

namespace TBCTask.Services;

public class RelatedPersonService : IRelatedPersonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly IValidator<RelatedPersonModel> _validator;

    public RelatedPersonService(IUnitOfWork unitOfWork, IValidator<RelatedPersonModel> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    #region Private Methods

    private RelatedPerson ModelToRelatedPerson(RelatedPersonModel model)
    {
        return new RelatedPerson
        {
            RelatedType = Enum.Parse<RelateType>(model.RelatedType),
            PersonID = model.PersonID,
            RelatedPersonID = model.RelatedPersonID
        };
    }

    #endregion

    public async Task<AddPersonResult> AddRelatedPerson(RelatedPersonModel entity)
    {
        if (entity != null)
        {
            var isValid = _validator.Validate(entity);
            if (isValid.IsValid)
            {
                _logger.Info("Creating Related Person");
                var model = ModelToRelatedPerson(entity);
                await _unitOfWork.RelatedPersons.AddAsync(model);
                return new AddPersonResult { IsSuccessful = true };
            }

            return new AddPersonResult { IsSuccessful = false, ValidationErrors = isValid.Errors };
        }

        return new AddPersonResult { IsSuccessful = false };
    }

    public async Task<bool> DeleteRelatedPerson(int id)
    {
        return await _unitOfWork.RelatedPersons.DeleteAsync(id);
    }
}