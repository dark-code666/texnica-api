namespace ERP.Api.Dtos;

public class ShipmentControlDto
{
    public int ID { get; set; }
    public string ShipmentNumber { get; set; } = string.Empty;
    public DateTime? PlannedLoadingDate { get; set; }
    public DateTime? ActualLoadingDate { get; set; }
    public DateTime? ETD { get; set; }
    public DateTime? ETA { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    // Style/Color/Size derivados de FgpoLines
    public string? Style { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal PlannedQty { get; set; }
    public decimal ActualLoadedQty { get; set; }
    public decimal InTransitQty { get; set; }
    public decimal CustomerReceivedQty { get; set; }
    public decimal TotalShippedQty { get; set; }
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
    public int? DataOwnerId { get; set; }
    public string? DataOwnerName { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
