namespace ERP.Api.Dtos;

public class MillProductionDto
{
    public int ID { get; set; }
    public int FabricPOId { get; set; }
    public string? FabricPONumber { get; set; }
    public int FGPOId { get; set; }
    public string? FGPONumber { get; set; }
    
    // Derivados de FabricPO
    public string? SupplierName { get; set; }
    public string? Supplier => SupplierName;
    public string? ComponentCode { get; set; }
    public string? FabricComponent => ComponentCode;

    public string? Style { get; set; }
    public string? Color { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal ProducedQuantity { get; set; }
    public decimal CompletionPercentage { get; set; }
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
    public string? DataOwnerName { get; set; }
    public string? DataOwner => DataOwnerName;
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
