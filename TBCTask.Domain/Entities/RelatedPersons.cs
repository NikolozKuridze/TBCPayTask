namespace TBCTask.Domain;

public class RelatedPerson : BaseEntity
{
    public int PersonID { get; set; }
    public RelateType RelatedType { get; set; }
    public int RelatedPersonID { get; set; }
    public virtual Person Person { get; set; } 
}