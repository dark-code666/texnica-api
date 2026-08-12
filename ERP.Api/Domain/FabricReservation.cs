namespace ERP.Api.Domain;

public class FabricReservation : BaseEntity
{
    public DateTime ReservationDate { get; set; }
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // FabricComponent: derivado de FabricPO.ComponentId → se obtiene por JOIN
    // UOM: derivado de FabricPO.UOM → se obtiene por JOIN

    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal ReleasedQuantity { get; set; }
    // Columna calculada por SQL: MAX(0, Reserved − Released)
    public decimal RemainingReservation { get; set; }
    public string? Status { get; set; }

    // Reservado por: FK a Users (usuario logueado)
    public int? ReservedByUserId { get; set; }
    public User? ReservedBy { get; set; }

    // Aprobado por: FK a Users
    public int? ApprovedByUserId { get; set; }
    public User? ApprovedBy { get; set; }

    public DateTime? LastUpdated { get; set; }
    public string? Comments { get; set; }
}
