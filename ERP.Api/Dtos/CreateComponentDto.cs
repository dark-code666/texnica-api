namespace ERP.Api.Dtos;

public class CreateComponentDto
{
    public string ComponentCode { get; set; } = null!;
    public string? Description { get; set; }
}
