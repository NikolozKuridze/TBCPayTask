using Microsoft.AspNetCore.Http;

namespace TBCTask.Domain.Models;

public class AddFileModel
{
    public int PersonID { get; set; }
    public IFormFile Image { get; set; }
}