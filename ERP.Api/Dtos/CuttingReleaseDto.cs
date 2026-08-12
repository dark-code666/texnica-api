namespace ERP.Api.Dtos;

public class CuttingReleaseDto
{
    public int ID { get; set; }
    public string ReleaseNumber { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Style { get; set; }
    public string? Color { get; set; }
    public string? FabricLot { get; set; }
    public decimal ApprovedCutQty { get; set; }
    public decimal ApprovedWidth { get; set; }
    public string? MarkerNumber { get; set; }
    public decimal ApprovedYield { get; set; }
    public string? PrrResult { get; set; }
    public string? ReleasedBy { get; set; }
    public string? ReviewedBy { get; set; }
    public string? Exception { get; set; }
    public string? Conditions { get; set; }
    public string? ReleaseStatus { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
