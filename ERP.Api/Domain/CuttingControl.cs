namespace ERP.Api.Domain;

public class CuttingControl : BaseEntity
{
    public DateTime CutDate { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Talla normalizada: FK a Sizes
    public int? SizeId { get; set; }
    public Size? Size { get; set; }

    public string? FabricLot { get; set; }
    public string? MarkerNumber { get; set; }
    public int PlannedCut { get; set; }
    public int ActualCut { get; set; }
    public int GoodCut { get; set; }
    public int DamagedQty { get; set; }
    public int ReplacementCut { get; set; }
    public int SentToSewing { get; set; }
    // Columnas calculadas por SQL (fórmulas del Excel)
    public int CuttingVariance { get; set; }
    public int PendingCut { get; set; }
    public int OvercutQty { get; set; }
    public int CutToSewDifference { get; set; }
    public string? ReleaseStatus { get; set; }

    // Responsable: FK a Users (usuario logueado)
    public int? ResponsiblePersonId { get; set; }
    public User? ResponsiblePerson { get; set; }

    public string? Comments { get; set; }
}
