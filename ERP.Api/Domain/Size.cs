namespace ERP.Api.Domain;

public class Size : BaseEntity
{
    public string SizeCode { get; set; } = null!;
    public int SortOrder { get; set; }
}
