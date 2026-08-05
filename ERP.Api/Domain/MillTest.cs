namespace ERP.Api.Domain;

public class MillTest : BaseEntity
{
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    public string? Supplier { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
    public string? Color { get; set; }
    public decimal RollQty { get; set; }
    public decimal ActualWidth { get; set; }
    public decimal ActualGSM { get; set; }
    public decimal LengthShrinkagePercentage { get; set; }
    public decimal WidthShrinkagePercentage { get; set; }
    public decimal TorquePercentage { get; set; }
    public decimal BowingPercentage { get; set; }
    public decimal SkewingPercentage { get; set; }
    public string? Colorfastness { get; set; }
    public string? WashAppearance { get; set; }
    public string? HandFeel { get; set; }
    public DateTime TestDate { get; set; }
    public string? TestedBy { get; set; }
    public string? TestResult { get; set; }
    public bool ApprovedForExport { get; set; }
    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
}
