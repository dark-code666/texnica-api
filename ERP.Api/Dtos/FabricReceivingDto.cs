namespace ERP.Api.Dtos;

public class FabricReceivingDto
{
    public int ID { get; set; }
    public string ReceivingNumber { get; set; } = null!;
    public DateTime ReceivingDate { get; set; }
    public string? ShipmentNumber { get; set; }
    public int FabricPOId { get; set; }
    public string FabricPONumber { get; set; } = null!;
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Supplier { get; set; }
    public decimal PackingListQty { get; set; }
    public decimal ActualReceivedQty { get; set; }
    public decimal ReceivingVariance { get; set; }
    public decimal ReceivingShortage { get; set; }
    public decimal ReceivingOverQty { get; set; }
    public int ExpectedRolls { get; set; }
    public int ReceivedRolls { get; set; }
    public int MissingRolls { get; set; }
    public string? ReceivingStatus { get; set; }
    public string? WarehouseLocation { get; set; }
    public string? ReceivedBy { get; set; }
    public string? DataOwner { get; set; }
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
