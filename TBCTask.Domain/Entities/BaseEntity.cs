using System.ComponentModel.DataAnnotations;

namespace TBCTask.Domain;

public class BaseEntity
{
    [Key]
    public int ID { get; set; }
}