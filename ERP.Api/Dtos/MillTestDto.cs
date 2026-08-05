namespace ERP.Api.Dtos;

public class MillTestDto
{
    public int ID { get; set; }
    public int FabricPOId { get; set; }
    public string FabricPONumber { get; set; } = null!;
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Supplier { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
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
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
