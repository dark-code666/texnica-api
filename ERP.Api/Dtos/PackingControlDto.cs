namespace ERP.Api.Dtos;

public class PackingControlDto
{
    public int ID { get; set; }
    public DateTime PackingDate { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    // Style/Color/Size derivados de FgpoLines
    public string? Style { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal QcPassedQty { get; set; }
    public decimal ReceivedByPackingQty { get; set; }
    public decimal FoldedQty { get; set; }
    public decimal PolybaggedQty { get; set; }
    public decimal PackedQty { get; set; }
    public int FullCartons { get; set; }
    public int PartialCartons { get; set; }
    public int PcsPerCarton { get; set; }
    public decimal ReadyToShipQty { get; set; }
    public decimal PackingVariance { get; set; }
    public decimal PendingPacking { get; set; }
    public decimal OverpackedQty { get; set; }
    public int? ResponsiblePersonId { get; set; }
    public string? ResponsiblePersonName { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
