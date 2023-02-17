namespace TBCTask.Domain;

public class PersonPhoneNumber : BaseEntity
{
    public int PersonID { get; set; }
    public string Number { get; set; }
    public NumberType Type { get; set; }
    public virtual Person Person { get; set; }
}