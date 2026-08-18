namespace ERP.Api.Domain;

public class PackingControl : BaseEntity
{
    public DateTime PackingDate { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Style/Color/Size: derivados de FgpoLines (primera línea activa) → se obtienen por JOIN

    public decimal QcPassedQty { get; set; }
    public decimal ReceivedByPackingQty { get; set; }
    public decimal FoldedQty { get; set; }
    public decimal PolybaggedQty { get; set; }
    public decimal PackedQty { get; set; }
    public int FullCartons { get; set; }
    public int PartialCartons { get; set; }
    public int PcsPerCarton { get; set; }

    // Columnas calculadas por SQL:
    // ReadyToShipQty = PackedQty
    // PackingVariance = PackedQty - QcPassedQty
    // PendingPacking  = MAX(0, QcPassedQty - PackedQty)
    // OverpackedQty   = MAX(0, PackedQty - QcPassedQty)
    public decimal ReadyToShipQty { get; set; }
    public decimal PackingVariance { get; set; }
    public decimal PendingPacking { get; set; }
    public decimal OverpackedQty { get; set; }

    // Responsable: FK a Users
    public int? ResponsiblePersonId { get; set; }
    public User? ResponsiblePerson { get; set; }

    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
}
