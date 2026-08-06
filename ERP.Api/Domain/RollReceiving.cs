namespace ERP.Api.Domain;

public class RollReceiving : BaseEntity
{
    public int ReceivingId { get; set; }
    public FabricReceiving? Receiving { get; set; }
    public string? ReceivingNumber { get; set; }
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    public string? Supplier { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
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
}
