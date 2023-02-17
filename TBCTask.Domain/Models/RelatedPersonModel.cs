namespace TBCTask.Domain.Models;

public class RelatedPersonModel
{
    public int? ID { get; set; }
    public int PersonID { get; set; }
    public string RelatedType { get; set; }
    public int RelatedPersonID { get; set; } 
}