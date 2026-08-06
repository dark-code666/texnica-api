namespace ERP.Api.Dtos;

public class UpdateInternalTestDto
{
    public DateTime TestDate { get; set; }
    public int FabricPOId { get; set; }
    public int FGPOId { get; set; }
    public string? Supplier { get; set; }
    public string? LotNumber { get; set; }
    public string? Color { get; set; }
    public decimal ActualWidth { get; set; }
    public decimal SpecimenAreaCm2 { get; set; }
    public decimal WeightBeforeG { get; set; }
    public decimal WeightAfterG { get; set; }
    public decimal TargetGSM { get; set; }
    public decimal LengthBefore { get; set; }
    public decimal LengthAfter { get; set; }
    public decimal WidthBefore { get; set; }
    public decimal WidthAfter { get; set; }
    public decimal TorquePct { get; set; }
    public decimal BowingPct { get; set; }
    public decimal SkewingPct { get; set; }
    public string? ShadeResult { get; set; }
    public string? WashAppearance { get; set; }
    public string? HandFeel { get; set; }
    public string? TestResult { get; set; }
    public string? TestedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
}
