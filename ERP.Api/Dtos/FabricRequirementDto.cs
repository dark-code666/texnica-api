namespace ERP.Api.Dtos;

public class FabricRequirementDto
{
    public int ID { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    // Style/Color se mantienen como texto descriptivo de la especificación técnica de la tela
    public string? Style { get; set; }
    public string? Color { get; set; }
    // FabricComponent normalizado a ComponentId
    public int? ComponentId { get; set; }
    public string? ComponentCode { get; set; }
    public string? FabricDescription { get; set; }
    public string? Composition { get; set; }
    public decimal GSM { get; set; }
    public string? RequiredWidth { get; set; }
    public string? UOM { get; set; }
    public decimal OrderQuantity { get; set; }
    public decimal ApprovedYield { get; set; }
    public decimal GrossRequirement { get; set; }
    public decimal AllowancePercentage { get; set; }
    public decimal AllowanceQty { get; set; }
    public decimal AvailableInventory { get; set; }
    public decimal NetPurchaseRequirement { get; set; }
    public DateTime RequiredDate { get; set; }
    public string? Status { get; set; }
    public int? DataOwnerId { get; set; }
    public string? DataOwnerName { get; set; }
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
