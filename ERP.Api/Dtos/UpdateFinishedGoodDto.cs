namespace ERP.Api.Dtos;

public class UpdateFinishedGoodDto
{
    public DateTime ReceiptDate { get; set; }
    public int FGPOId { get; set; }
    public decimal PackedQty { get; set; }
    public decimal WarehouseReceived { get; set; }
    public decimal ReservedForShipment { get; set; }
    public decimal LoadedQty { get; set; }
    public decimal ShippedQty { get; set; }
    public string? WarehouseLocation { get; set; }
    public string? Status { get; set; }
    public int? DataOwnerId { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
}
