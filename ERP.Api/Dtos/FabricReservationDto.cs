namespace ERP.Api.Dtos;

public class FabricReservationDto
{
    public int ID { get; set; }
    public DateTime ReservationDate { get; set; }
    public int FabricPOId { get; set; }
    public string FabricPONumber { get; set; } = string.Empty;
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    // FabricComponent derivado de FabricPO.Component
    public int? ComponentId { get; set; }
    public string? ComponentCode { get; set; }
    public int? LotId { get; set; }
    public string? LotNumber { get; set; }
    public decimal ReservedQuantity { get; set; }
    // UOM derivado de FabricPO.UOM
    public string? UOM { get; set; }
    public decimal ReleasedQuantity { get; set; }
    public decimal RemainingReservation { get; set; }
    public string? Status { get; set; }
    public int? ReservedByUserId { get; set; }
    public string? ReservedByName { get; set; }
    public int? ApprovedByUserId { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
