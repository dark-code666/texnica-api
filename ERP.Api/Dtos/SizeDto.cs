namespace ERP.Api.Dtos;

public class SizeDto
{
    public int ID { get; set; }
    public string SizeCode { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
