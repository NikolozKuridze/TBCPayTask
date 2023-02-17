namespace TBCTask.Domain.Models;

public class PersonPhoneNumberModel
{
    public int ID { get; set; }
    public int PersonID { get; set; }
    public string Number { get; set; }
    public string NumberType { get; set; }
}