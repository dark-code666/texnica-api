namespace ERP.Api.Dtos;

public class CreateMillProductionDto
{
    public int FabricPOId { get; set; }
    public int FGPOId { get; set; }
    public string? Style { get; set; }
    public string? Color { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal ProducedQuantity { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public decimal RollQuantity { get; set; }
    public decimal YardageOrQty { get; set; }
    public decimal Weight { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public DateTime? PlannedExport { get; set; }
    public DateTime? ActualExport { get; set; }
    public string? Status { get; set; }
    public int? DataOwnerId { get; set; }
    public string? Remarks { get; set; }
}
