namespace ERP.Api.Dtos;

public class InlineQualityDto
{
    public int ID { get; set; }
    public DateTime InspectionDate { get; set; }
    public string? Time { get; set; }
    public string? Line { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Style { get; set; }
    public string? Color { get; set; }
    public string? Operation { get; set; }
    public string? Operator { get; set; }
    public int CheckedQty { get; set; }
    public int CriticalDefects { get; set; }
    public int MajorDefects { get; set; }
    public int MinorDefects { get; set; }
    public int TotalDefects { get; set; }
    public decimal DhuPct { get; set; }
    public int DefectivePieces { get; set; }
    public decimal DefectiveRatePct { get; set; }
    public decimal MaxAllowed { get; set; }
    public string? Result { get; set; }
    public string? Inspector { get; set; }
    public string? ImmediateCorrection { get; set; }
    public string? RootCause { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
