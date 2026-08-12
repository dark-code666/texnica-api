namespace ERP.Api.Domain;

public class InlineQuality : BaseEntity
{
    public DateTime InspectionDate { get; set; }
    public string? Time { get; set; }
    public string? Line { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    public string? Operation { get; set; }
    public string? Operator { get; set; }
    public int CheckedQty { get; set; }
    public int CriticalDefects { get; set; }
    public int MajorDefects { get; set; }
    public int MinorDefects { get; set; }
    // Columnas calculadas por SQL
    public int TotalDefects { get; set; }
    public decimal DhuPct { get; set; }
    public int DefectivePieces { get; set; }
    public decimal DefectiveRatePct { get; set; }
    public decimal MaxAllowed { get; set; }
    public string? Result { get; set; }

    // Inspector: FK a Users (usuario logueado)
    public int? InspectorId { get; set; }
    public User? Inspector { get; set; }

    public string? ImmediateCorrection { get; set; }
    public string? RootCause { get; set; }
}
