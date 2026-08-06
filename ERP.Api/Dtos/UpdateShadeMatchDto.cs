namespace ERP.Api.Dtos;

public class UpdateShadeMatchDto
{
    public DateTime ReviewDate { get; set; }
    public int FGPOId { get; set; }
    public string? BodyFabricLot { get; set; }
    public string? RibLot { get; set; }
    public string? ShoulderTapeLot { get; set; }
    public string? BodyShadeGroup { get; set; }
    public string? RibShadeGroup { get; set; }
    public string? TapeShadeGroup { get; set; }
    public string? BodyVsRib { get; set; }
    public string? BodyVsTape { get; set; }
    public string? LightSource { get; set; }
    public string? BeforeWashResult { get; set; }
    public string? AfterWashResult { get; set; }
    public string? OverallResult { get; set; }
    public string? ApprovedBy { get; set; }
    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
}
