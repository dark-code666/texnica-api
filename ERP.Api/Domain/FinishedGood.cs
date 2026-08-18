namespace ERP.Api.Domain;

public class FinishedGood : BaseEntity
{
    public DateTime ReceiptDate { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Style/Color/Size: derivados de FgpoLines (primera línea activa) → se obtienen por JOIN

    public decimal PackedQty { get; set; }
    public decimal WarehouseReceived { get; set; }
    public decimal ReservedForShipment { get; set; }
    public decimal LoadedQty { get; set; }
    public decimal ShippedQty { get; set; }

    // Columnas calculadas por SQL:
    // ReadyToShipQty   = MAX(0, WarehouseReceived - ReservedForShipment - LoadedQty - ShippedQty)
    // WarehouseBalance = MAX(0, WarehouseReceived - LoadedQty - ShippedQty)
    public decimal ReadyToShipQty { get; set; }
    public decimal WarehouseBalance { get; set; }

    public string? WarehouseLocation { get; set; }
    public string? Status { get; set; }

    // Dueño del dato: FK a Users
    public int? DataOwnerId { get; set; }
    public User? DataOwner { get; set; }

    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
}
