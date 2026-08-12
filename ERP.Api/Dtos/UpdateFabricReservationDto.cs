namespace ERP.Api.Dtos;

public class UpdateFabricReservationDto
{
    public DateTime ReservationDate { get; set; }
    public int FabricPOId { get; set; }
    public int FGPOId { get; set; }
    public int? LotId { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal ReleasedQuantity { get; set; }
    public string? Status { get; set; }
    public int? ReservedByUserId { get; set; }
    public int? ApprovedByUserId { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Comments { get; set; }
}
