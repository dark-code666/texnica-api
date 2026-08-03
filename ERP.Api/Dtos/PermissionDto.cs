namespace ERP.Api.Dtos;

public class PermissionDto
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Module { get; set; } = null!;
    public bool Active { get; set; }
}
