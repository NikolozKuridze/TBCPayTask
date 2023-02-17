using Microsoft.AspNetCore.Mvc;
using TBCTask.Domain.Interfaces.IServices;
using TBCTask.Domain.Models;
using TBCTask.API.ActionFilters;

namespace TBCTask.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private IPersonService _personService;
    private IWebHostEnvironment _environment;

    public PersonController(IPersonService personService, IWebHostEnvironment environment)
    {
        _personService = personService;
        _environment = environment;
    }
 
    [HttpPost]
    [Route("fileupload")]
    public async Task<IActionResult> FileUpload([FromForm] AddFileModel model)
    {
        var person = _personService.GetPersonById(model.PersonID).Result;
        if (person.ID == model.PersonID)
        {
            if (model.Image == null || model.Image.Length == 0)
                return BadRequest("Invalid file");

            if (!model.Image.ContentType.StartsWith("image/"))
                return BadRequest("Invalid file type");

            var fileName = Path.GetFileNameWithoutExtension(model.Image.FileName);
            var extension = Path.GetExtension(model.Image.FileName);
            var uniqueFileName = $"{fileName}_{DateTime.Now.Ticks}{extension}";
            var relativePath = Path.Combine("images", uniqueFileName);
            var filePath = Path.Combine(_environment.ContentRootPath, relativePath);
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await _personService.UpdateImage(relativePath, person.ID);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            return Ok(relativePath);
        }

        return NotFound("Person Not Found");
    }

    [HttpPost]
    [Route("Add")]
    [ValidateModel]
    public async Task<IActionResult> AddPerson(PersonModel model)
    {
        var result = _personService.AddPerson(model).Result;
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

    [HttpGet("GetByID")]
    public async Task<PersonModel> GetPersonById(int id)
    {
        return _personService.GetPersonById(id).Result;
    }

    [HttpDelete]
    public async Task<bool> DeletePersonById(int id)
    {
        return _personService.DeletePerson(id).Result;
    }

    [HttpGet("All")] 
    public async Task<List<PersonModel>> GetAll()
    {
        var list = _personService.GetAll().Result.ToList();
        return list;
    }

    [HttpGet("FastSearch")] 
    public async Task<List<PersonModel>> FastSearch(string searchPattern)
    {
        var list = _personService.FastSearch(searchPattern).Result;
        return list;
    }

    [HttpGet("SearchWithPaging")] 
    public async Task<List<PersonModel>> SearchWithPaging(int pageIndex, int pageSize, string searchPattern)
    {
        var list = _personService.PagedSearchPersons(pageIndex, pageSize, searchPattern).Result;
        return list;
    }
}