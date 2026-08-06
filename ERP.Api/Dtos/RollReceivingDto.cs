namespace ERP.Api.Dtos;

public class RollReceivingDto
{
    public int ID { get; set; }
    public int ReceivingId { get; set; }
    public string ReceivingNumber { get; set; } = null!;
    public int FabricPOId { get; set; }
    public string FabricPONumber { get; set; } = null!;
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Supplier { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public string? RollNumber { get; set; }
    public string? SupplierRollNumber { get; set; }
    public string? Color { get; set; }
    public decimal GrossWeight { get; set; }
    public decimal NetWeight { get; set; }
    public decimal ActualYardage { get; set; }
    public decimal ActualWidth { get; set; }
    public decimal ActualGSM { get; set; }
    public string? ShadeGroup { get; set; }
    public decimal DamagedQty { get; set; }
    public string? Condition { get; set; }
    public string? WarehouseLocation { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string? DataOwner { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
