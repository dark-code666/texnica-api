namespace ERP.Api.Dtos;

public class UpdateTopSampleDto
{
    public int FGPOId { get; set; }
    public int? SizeId { get; set; }
    public string? ProductionLine { get; set; }
    public string? FabricLot { get; set; }
    public string? CutLotBundle { get; set; }
    public string? TrimVersion { get; set; }
    public string? ThreadLot { get; set; }
    public int TopQty { get; set; }
    public DateTime? ProductionDate { get; set; }
    public string? MeasurementResult { get; set; }
    public string? ConstructionResult { get; set; }
    public string? WorkmanshipResult { get; set; }
    public string? LabelResult { get; set; }
    public string? PackingResult { get; set; }
    public string? InternalReview { get; set; }
    public string? CustomerReview { get; set; }
    public string? CorrectiveAction { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public int? ApprovedByUserId { get; set; }
    public string? Status { get; set; }
    public string? DocumentLink { get; set; }
    public string? PhotoLink { get; set; }
}
