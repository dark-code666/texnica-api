namespace ERP.Api.Dtos;

public class CreateStyleDto
{
    public string StyleCode { get; set; } = null!;
    public string? Description { get; set; }
    public string? FabricDescription { get; set; }
    public string? FabricContent { get; set; }
    public string? Construction { get; set; }
    public decimal? Gsm { get; set; }
    public decimal? WeightOz { get; set; }
    public string? Comments { get; set; }
}
