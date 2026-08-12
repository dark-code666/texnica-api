namespace ERP.Api.Dtos;

public class PpSampleDto
{
    public int ID { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Style { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public string? SampleVersion { get; set; }
    public string? FabricLot { get; set; }
    public string? TrimVersion { get; set; }
    public DateTime? PreparationDate { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public string? MeasurementResult { get; set; }
    public string? ConstructionResult { get; set; }
    public string? FitResult { get; set; }
    public string? FabricResult { get; set; }
    public string? TrimResult { get; set; }
    public string? LabelResult { get; set; }
    public string? InternalReview { get; set; }
    public string? CustomerReview { get; set; }
    public string? CustomerComments { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? ApprovedBy { get; set; }
    public string? Status { get; set; }
    public string? DocumentLink { get; set; }
    public string? PhotoLink { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
