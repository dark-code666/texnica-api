namespace ERP.Api.Dtos;

public class StyleDto
{
    public int ID { get; set; }
    public string StyleCode { get; set; } = null!;
    public string? Description { get; set; }
    public string? FabricDescription { get; set; }
    public string? FabricContent { get; set; }
    public string? Construction { get; set; }
    public decimal? Gsm { get; set; }
    public decimal? WeightOz { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
