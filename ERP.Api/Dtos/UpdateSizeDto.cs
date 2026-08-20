namespace ERP.Api.Dtos;

public class UpdateSizeDto
{
    public string SizeCode { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}
