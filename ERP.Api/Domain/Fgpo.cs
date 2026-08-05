namespace ERP.Api.Domain;

public class Fgpo : BaseEntity
{
    public string FGPONumber { get; set; } = null!;
    public string? TemporaryNumber { get; set; }
    public string? Status { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string? Style { get; set; }
    public string? Color { get; set; }
    public int OrderQuantity { get; set; }
    public DateTime DeliveryDate { get; set; }
    public decimal InTransitQty { get; set; }
    public decimal ReceivedQty { get; set; }
    public decimal TotalShippedQty { get; set; }
    public decimal ShipmentVariance { get; set; }
    public decimal PendingToShip { get; set; }
    public decimal OvershipmentQty { get; set; }
    public decimal ProducedQty { get; set; }
    public decimal ProductionVariance { get; set; }
    public decimal PendingProduction { get; set; }
    public decimal OverproductionQty { get; set; }
    public string? DataOwner { get; set; }
    public string? Remarks { get; set; }

    // Relación inversa: un FGPO puede estar cubierto por varios Fabric PO
    public ICollection<FabricPOFgpo> FabricPOFgpos { get; set; } = new List<FabricPOFgpo>();
}
