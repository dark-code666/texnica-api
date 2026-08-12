namespace ERP.Api.Domain;

public class FabricShipment : BaseEntity
{
    public string ShipmentNumber { get; set; } = null!;
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Proveedor: derivado de FabricPO.SupplierId → se obtiene por JOIN

    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
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

    // Dueño del dato: FK a Users (usuario logueado)
    public int? DataOwnerId { get; set; }
    public User? DataOwner { get; set; }

    public string? Remarks { get; set; }
}
