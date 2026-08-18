namespace ERP.Api.Domain;

public class ShipmentControl : BaseEntity
{
    public string ShipmentNumber { get; set; } = null!;
    public DateTime? PlannedLoadingDate { get; set; }
    public DateTime? ActualLoadingDate { get; set; }
    public DateTime? ETD { get; set; }
    public DateTime? ETA { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Customer, Style/Color/Size: derivados de FGPO / FgpoLines → se obtienen por JOIN

    public decimal PlannedQty { get; set; }
    public decimal ActualLoadedQty { get; set; }
    public decimal InTransitQty { get; set; }
    public decimal CustomerReceivedQty { get; set; }
    public decimal TotalShippedQty { get; set; }

    // Columnas calculadas por SQL:
    // ShipmentVariance = TotalShippedQty - PlannedQty
    // PendingToShip    = MAX(0, PlannedQty - TotalShippedQty)
    // OvershipmentQty  = MAX(0, TotalShippedQty - PlannedQty)
    public decimal ShipmentVariance { get; set; }
    public decimal PendingToShip { get; set; }
    public decimal OvershipmentQty { get; set; }

    public string? ContainerType { get; set; }
    public string? ContainerNumber { get; set; }
    public string? BookingNumber { get; set; }
    public string? Destination { get; set; }
    public string? ShipmentStatus { get; set; }
    public string? PackingList { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? LoadPlan { get; set; }

    // Dueño del dato: FK a Users
    public int? DataOwnerId { get; set; }
    public User? DataOwner { get; set; }

    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
}
