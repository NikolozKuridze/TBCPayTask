using Microsoft.AspNetCore.Mvc;
using TBCTask.Domain.Interfaces.IServices;
using TBCTask.Domain.Models;

namespace TBCTask.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RelatedPersonController : ControllerBase
{
    private IRelatedPersonService _relatedPersonService;

    public RelatedPersonController(IRelatedPersonService personService)
    {
        _relatedPersonService = personService;
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> AddPerson(RelatedPersonModel model)
    {
        var result = _relatedPersonService.AddRelatedPerson(model).Result;
        if (result.IsSuccessful)
        {
            return Ok();
        }

        if (result.ValidationErrors != null)
        {
            return BadRequest(result.ValidationErrors);
        }

        return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
    }

    [HttpDelete]
    public async Task<bool> DeletePersonById(int id)
    {
        return _relatedPersonService.DeleteRelatedPerson(id).Result;
    }
}