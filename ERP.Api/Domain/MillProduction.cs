namespace ERP.Api.Domain;

public class MillProduction : BaseEntity
{
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    public string? Supplier { get; set; }
    public string? FabricComponent { get; set; }
    public string? Style { get; set; }
    public string? Color { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal ProducedQuantity { get; set; }
    public decimal CompletionPercentage { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
    public decimal RollQuantity { get; set; }
    public decimal YardageOrQty { get; set; }
    public decimal Weight { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public DateTime? PlannedExport { get; set; }
    public DateTime? ActualExport { get; set; }
    public string? Status { get; set; }
    public string? DataOwner { get; set; }
    public string? Remarks { get; set; }
}
