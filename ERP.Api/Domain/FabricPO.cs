namespace ERP.Api.Domain;

public class FabricPO : BaseEntity
{
    public string FabricPONumber { get; set; } = null!;
    public string? Supplier { get; set; }
    public string? FabricMill { get; set; }
    public string? FabricComponent { get; set; }
    public decimal OrderedQuantity { get; set; }
    public string? UOM { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal POAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredCompletion { get; set; }
    public DateTime? PlannedExport { get; set; }
    public DateTime? PlannedArrival { get; set; }
    public string? POStatus { get; set; }
    public string? PurchaseOwner { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Relación muchos-a-muchos con FGPO (un Fabric PO puede cubrir varios FGPO)
    public ICollection<FabricPOFgpo> FabricPOFgpos { get; set; } = new List<FabricPOFgpo>();
}
