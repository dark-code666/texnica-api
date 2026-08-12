namespace ERP.Api.Dtos;

public class CreateCuttingReleaseDto
{
    public DateTime ReleaseDate { get; set; }
    public int FGPOId { get; set; }
    public string? FabricLot { get; set; }
    public decimal ApprovedCutQty { get; set; }
    public decimal ApprovedWidth { get; set; }
    public string? MarkerNumber { get; set; }
    public decimal ApprovedYield { get; set; }
    public string? PrrResult { get; set; }
    public int? ReleasedByUserId { get; set; }
    public int? ReviewedByUserId { get; set; }
    public string? Exception { get; set; }
    public string? Conditions { get; set; }
    public string? ReleaseStatus { get; set; }
    public string? Comments { get; set; }
}
