namespace ERP.Api.Domain;

public class Fgpo : BaseEntity
{
    public string FGPONumber { get; set; } = null!;
    public string? TemporaryNumber { get; set; }
    public string? Status { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    // Style y Color eliminados: datos reales viven en FgpoLines (FK a Styles/Colors)
    // Si se necesita el estilo/color "primario" del FGPO, se obtiene por JOIN a FgpoLines

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

    // Dueño del dato: FK a Users (usuario logueado al crear/modificar)
    public int? DataOwnerId { get; set; }
    public User? DataOwner { get; set; }

    public string? Remarks { get; set; }

    // Relación inversa: un FGPO puede estar cubierto por varios Fabric PO
    public ICollection<FabricPOFgpo> FabricPOFgpos { get; set; } = new List<FabricPOFgpo>();

    // Relación inversa: un FGPO tiene líneas (Style + Color + Size)
    public ICollection<FgpoLine> FgpoLines { get; set; } = new List<FgpoLine>();
}
