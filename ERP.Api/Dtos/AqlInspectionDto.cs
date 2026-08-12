namespace ERP.Api.Dtos;

public class AqlInspectionDto
{
    public int ID { get; set; }
    public string InspectionType { get; set; } = null!;
    public DateTime InspectionDate { get; set; }
    public int FGPOId { get; set; }
    public string? FgpoNumber { get; set; }
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
    public int? InspectorId { get; set; }
    public string? InspectorName { get; set; }
    public string? Disposition { get; set; }
    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
