namespace ERP.Api.Domain;

public class BaseEntity
{
    public int ID { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
