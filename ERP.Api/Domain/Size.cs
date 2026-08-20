namespace ERP.Api.Domain;

public class Size : BaseEntity
{
    public string SizeCode { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}
