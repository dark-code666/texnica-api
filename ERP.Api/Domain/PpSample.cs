namespace ERP.Api.Domain;

public class PpSample : BaseEntity
{
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Talla normalizada: FK a Sizes
    public int? SizeId { get; set; }
    public Size? Size { get; set; }

    public string? SampleVersion { get; set; }
    public string? FabricLot { get; set; }
    public string? TrimVersion { get; set; }
    public DateTime? PreparationDate { get; set; }
    public DateTime? SubmissionDate { get; set; }
    // Resultados por rubro (dropdown Excel: Pending, Passed, Failed, Approved with Comments, N/A)
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

    // Aprobado por: FK a Users
    public int? ApprovedByUserId { get; set; }
    public User? ApprovedBy { get; set; }

    public string? Status { get; set; }
    public string? DocumentLink { get; set; }
    public string? PhotoLink { get; set; }
}
