namespace ERP.Api.Dtos;

public class CreateFabricPODto
{
    public string FabricPONumber { get; set; } = null!;
    public List<FabricPOFgpoItemDto> FgpoItems { get; set; } = new List<FabricPOFgpoItemDto>();
    public int? SupplierId { get; set; }
    public string? FabricMill { get; set; }
    public int? ComponentId { get; set; }
    public decimal OrderedQuantity { get; set; }
    public string? UOM { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredCompletion { get; set; }
    public DateTime? PlannedExport { get; set; }
    public DateTime? PlannedArrival { get; set; }
    public string? POStatus { get; set; }
    public int? PurchaseOwnerUserId { get; set; }
    public int? ApprovedByUserId { get; set; }
    public string? Remarks { get; set; }
}

public class FabricPOFgpoItemDto
{
    public int FGPOId { get; set; }
    public string? Style { get; set; }
    public string? Color { get; set; }
    public decimal AllocatedQuantity { get; set; }
}
