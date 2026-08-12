namespace ERP.Api.Domain;

public class InternalTest : BaseEntity
{
    public DateTime TestDate { get; set; }
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Proveedor: derivado de FabricPO.SupplierId → se obtiene por JOIN

    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
    public string? Color { get; set; }
    public decimal ActualWidth { get; set; }
    public decimal SpecimenAreaCm2 { get; set; }
    public decimal WeightBeforeG { get; set; }
    public decimal WeightAfterG { get; set; }
    public decimal TargetGSM { get; set; }
    // Columnas calculadas por SQL
    public decimal GsmBefore { get; set; }
    public decimal GsmAfter { get; set; }
    public decimal GsmVariancePct { get; set; }
    public decimal LengthBefore { get; set; }
    public decimal LengthAfter { get; set; }
    public decimal LengthShrinkagePct { get; set; }
    public decimal WidthBefore { get; set; }
    public decimal WidthAfter { get; set; }
    public decimal WidthShrinkagePct { get; set; }
    public decimal TorquePct { get; set; }
    public decimal BowingPct { get; set; }
    public decimal SkewingPct { get; set; }
    public string? ShadeResult { get; set; }
    public string? WashAppearance { get; set; }
    public string? HandFeel { get; set; }
    public string? TestResult { get; set; }

    // Testeado por: FK a Users (usuario logueado)
    public int? TestedByUserId { get; set; }
    public User? TestedBy { get; set; }

    // Aprobado por: FK a Users
    public int? ApprovedByUserId { get; set; }
    public User? ApprovedBy { get; set; }

    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
}
