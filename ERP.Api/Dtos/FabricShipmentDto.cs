namespace ERP.Api.Dtos;

public class FabricShipmentDto
{
    public int ID { get; set; }
    public string ShipmentNumber { get; set; } = null!;
    public int FabricPOId { get; set; }
    public string FabricPONumber { get; set; } = null!;
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Supplier { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
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
    public decimal InTransitQuantity { get; set; }
    public decimal RemainingToDeliver { get; set; }
    public string? DataOwner { get; set; }
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
