namespace ERP.Api.Dtos;

public class FinishedGoodDto
{
    public int ID { get; set; }
    public DateTime ReceiptDate { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    // Style/Color/Size derivados de FgpoLines
    public string? Style { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal PackedQty { get; set; }
    public decimal WarehouseReceived { get; set; }
    public decimal ReservedForShipment { get; set; }
    public decimal LoadedQty { get; set; }
    public decimal ShippedQty { get; set; }
    public decimal ReadyToShipQty { get; set; }
    public decimal WarehouseBalance { get; set; }
    public string? WarehouseLocation { get; set; }
    public string? Status { get; set; }
    public int? DataOwnerId { get; set; }
    public string? DataOwnerName { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
