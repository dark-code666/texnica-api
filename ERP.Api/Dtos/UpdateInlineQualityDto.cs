namespace ERP.Api.Dtos;

public class UpdateInlineQualityDto
{
    public DateTime InspectionDate { get; set; }
    public string? Time { get; set; }
    public string? Line { get; set; }
    public int FGPOId { get; set; }
    public string? Operation { get; set; }
    public string? Operator { get; set; }
    public int CheckedQty { get; set; }
    public int CriticalDefects { get; set; }
    public int MajorDefects { get; set; }
    public int MinorDefects { get; set; }
    public int DefectivePieces { get; set; }
    public decimal MaxAllowed { get; set; }
    public int? InspectorId { get; set; }
    public string? ImmediateCorrection { get; set; }
    public string? RootCause { get; set; }
}
