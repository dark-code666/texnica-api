namespace ERP.Api.Dtos;

public class UpdateFabricShipmentDto
{
    public string ShipmentNumber { get; set; } = null!;
    public int FabricPOId { get; set; }
    public int FGPOId { get; set; }
    public string? Supplier { get; set; }
    public string? LotNumber { get; set; }
    public decimal RollQty { get; set; }
    public decimal ShippedQuantity { get; set; }
    public string? UOM { get; set; }
    public decimal ShippedWeight { get; set; }
    public string? PackingList { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? ContainerAWB { get; set; }
    public string? ShippingMethod { get; set; }
    public DateTime ETD { get; set; }
    public DateTime ETA { get; set; }
    public string? ShipmentStatus { get; set; }
    public DateTime? DeliveredToTexnicaDate { get; set; }
    public string? DataOwner { get; set; }
    public string? Remarks { get; set; }
}
