namespace ERP.Api.Dtos;

public class FabricInventoryDto
{
    public int ID { get; set; }
    public int FabricPOId { get; set; }
    public string FabricPONumber { get; set; } = string.Empty;
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    // FabricComponent derivado de FabricPO.Component
    public int? ComponentId { get; set; }
    public string? ComponentCode { get; set; }
    public int? LotId { get; set; }
    public string? LotNumber { get; set; }
    // UOM derivado de FabricPO.UOM
    public string? UOM { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public decimal ApprovedQuantity { get; set; }
    public decimal RejectedQuantity { get; set; }
    public decimal HoldQuantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal IssuedQuantity { get; set; }
    public decimal ReturnedQuantity { get; set; }
    public decimal AvailableQuantity { get; set; }
    public decimal ShortageQuantity { get; set; }
    public string? WarehouseLocation { get; set; }
    public string? InventoryStatus { get; set; }
    public int? DataOwnerId { get; set; }
    public string? DataOwnerName { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
