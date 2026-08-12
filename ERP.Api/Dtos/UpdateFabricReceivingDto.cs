namespace ERP.Api.Dtos;

public class UpdateFabricReceivingDto
{
    public string ReceivingNumber { get; set; } = null!;
    public DateTime ReceivingDate { get; set; }
    public string? ShipmentNumber { get; set; }
    public int FabricPOId { get; set; }
    public int FGPOId { get; set; }
    // Supplier se deriva de FabricPO.Supplier — no se envía en el DTO
    public decimal PackingListQty { get; set; }
    public decimal ActualReceivedQty { get; set; }
    public int ExpectedRolls { get; set; }
    public int ReceivedRolls { get; set; }
    public string? ReceivingStatus { get; set; }
    public string? WarehouseLocation { get; set; }
    public int? ReceivedByUserId { get; set; }
    public int? DataOwnerId { get; set; }
    public string? Remarks { get; set; }
}
