namespace ERP.Api.Dtos;

public class FabricPODto
{
    public int ID { get; set; }
    public string FabricPONumber { get; set; } = null!;
    public List<FabricPOFgpoDto> Fgpos { get; set; } = new List<FabricPOFgpoDto>();
    
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Supplier => SupplierName;

    public string? FabricMill { get; set; }

    public int? ComponentId { get; set; }
    public string? ComponentCode { get; set; }
    public string? FabricComponent => ComponentCode;

    public decimal OrderedQuantity { get; set; }
    public string? UOM { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal POAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredCompletion { get; set; }
    public DateTime? PlannedExport { get; set; }
    public DateTime? PlannedArrival { get; set; }
    public string? POStatus { get; set; }

    public int? PurchaseOwnerUserId { get; set; }
    public string? PurchaseOwnerName { get; set; }
    public string? PurchaseOwner => PurchaseOwnerName;

    public int? ApprovedByUserId { get; set; }
    public string? ApprovedByName { get; set; }
    public string? ApprovedBy => ApprovedByName;

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
