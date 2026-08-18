namespace ERP.Api.Dtos;

public class UpdateShipmentControlDto
{
    public string ShipmentNumber { get; set; } = null!;
    public DateTime? PlannedLoadingDate { get; set; }
    public DateTime? ActualLoadingDate { get; set; }
    public DateTime? ETD { get; set; }
    public DateTime? ETA { get; set; }
    public int FGPOId { get; set; }
    public decimal PlannedQty { get; set; }
    public decimal ActualLoadedQty { get; set; }
    public decimal InTransitQty { get; set; }
    public decimal CustomerReceivedQty { get; set; }
    public decimal TotalShippedQty { get; set; }
    public string? ContainerType { get; set; }
    public string? ContainerNumber { get; set; }
    public string? BookingNumber { get; set; }
    public string? Destination { get; set; }
    public string? ShipmentStatus { get; set; }
    public string? PackingList { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? LoadPlan { get; set; }
    public int? DataOwnerId { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
}
