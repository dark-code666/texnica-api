namespace ERP.Api.Domain;

public class FabricInventory : BaseEntity
{
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // FabricComponent: derivado de FabricPO.ComponentId → se obtiene por JOIN
    // UOM: derivado de FabricPO.UOM → se obtiene por JOIN

    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public decimal ApprovedQuantity { get; set; }
    public decimal RejectedQuantity { get; set; }
    public decimal HoldQuantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal IssuedQuantity { get; set; }
    public decimal ReturnedQuantity { get; set; }
    // Columna calculada por SQL: MAX(0, Approved − Reserved − Issued + Returned)
    public decimal AvailableQuantity { get; set; }
    public decimal ShortageQuantity { get; set; }
    public string? WarehouseLocation { get; set; }
    public string? InventoryStatus { get; set; }

    // Dueño del dato: FK a Users (usuario logueado)
    public int? DataOwnerId { get; set; }
    public User? DataOwner { get; set; }

    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
}
