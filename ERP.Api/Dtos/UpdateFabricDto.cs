namespace ERP.Api.Dtos;

public class UpdateFabricDto
{
    public string? FabricReference { get; set; }
    public string FabricName { get; set; } = null!;
    public string? Color { get; set; }
    public string? Content { get; set; }
    public string? Construction { get; set; }
    public decimal? Gsm { get; set; }
    public decimal? WeightOz { get; set; }
    public string? Comments { get; set; }
}
