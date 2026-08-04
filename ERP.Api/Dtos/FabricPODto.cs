namespace ERP.Api.Dtos;

public class FabricPODto
{
    public int ID { get; set; }
    public string FabricPONumber { get; set; } = null!;
    public List<FabricPOFgpoDto> Fgpos { get; set; } = new List<FabricPOFgpoDto>();
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
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FabricPOFgpoDto
{
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Style { get; set; }
    public string? Color { get; set; }
    public decimal AllocatedQuantity { get; set; }
}
