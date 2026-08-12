namespace ERP.Api.Domain;

/// <summary>
/// Unificación de EndlineInspection, PreFinalInspection y FinalInspection.
/// Todas tienen estructura AQL idéntica. El campo InspectionType discrimina el tipo.
/// </summary>
public class AqlInspection : BaseEntity
{
    /// <summary>
    /// Tipo de inspección: "Endline" | "PreFinal" | "Final"
    /// </summary>
    public string InspectionType { get; set; } = null!;

    public DateTime InspectionDate { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    public string? LotShipment { get; set; }
    public int LotSize { get; set; }
    public string? InspectionLevel { get; set; }
    public decimal AqlMajor { get; set; }
    public decimal AqlMinor { get; set; }
    public int SampleSize { get; set; }
    public int CriticalDefects { get; set; }
    public int MajorDefects { get; set; }
    public int MinorDefects { get; set; }
    public int CriticalAc { get; set; }
    public int MajorAc { get; set; }
    public int MinorAc { get; set; }
    public int CriticalRe { get; set; }
    public int MajorRe { get; set; }
    public int MinorRe { get; set; }
    public string? Result { get; set; }

    // Inspector: FK a Users (usuario logueado)
    public int? InspectorId { get; set; }
    public User? Inspector { get; set; }

    public string? Disposition { get; set; }
    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
}
