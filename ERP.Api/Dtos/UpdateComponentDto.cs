namespace ERP.Api.Dtos;

public class UpdateComponentDto
{
    public string ComponentCode { get; set; } = null!;
    public string? Description { get; set; }
}
