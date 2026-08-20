namespace ERP.Api.Dtos;

public class CreateSizeDto
{
    public string SizeCode { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}
