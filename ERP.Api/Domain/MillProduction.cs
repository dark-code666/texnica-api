namespace ERP.Api.Domain;

public class MillProduction : BaseEntity
{
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Proveedor: derivado de FabricPO.SupplierId → se obtiene por JOIN, no se almacena redundante
    // FabricComponent: derivado de FabricPO.ComponentId → se obtiene por JOIN

    // Style y Color se mantienen como descripción del lote de producción en el molino
    public string? Style { get; set; }
    public string? Color { get; set; }

    public decimal PlannedQuantity { get; set; }
    public decimal ProducedQuantity { get; set; }
    public decimal CompletionPercentage { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
    public decimal RollQuantity { get; set; }
    public decimal YardageOrQty { get; set; }
    public decimal Weight { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public DateTime? PlannedExport { get; set; }
    public DateTime? ActualExport { get; set; }
    public string? Status { get; set; }

    // Dueño del dato: FK a Users (usuario logueado)
    public int? DataOwnerId { get; set; }
    public User? DataOwner { get; set; }

    public string? Remarks { get; set; }
}
