namespace ERP.Api.Dtos;

public class ProductionReadinessDto
{
    public int ID { get; set; }
    public DateTime ReviewDate { get; set; }
    public int FGPOId { get; set; }
    public string? FgpoNumber { get; set; }
    public string? PoConfirmed { get; set; }
    public string? TechPackCurrent { get; set; }
    public string? FabricApproved { get; set; }
    public string? TrimsApproved { get; set; }
    public string? TrimsAvailable { get; set; }
    public string? PpSampleApproved { get; set; }
    public string? PatternApproved { get; set; }
    public string? MarkerApproved { get; set; }
    public string? FabricWidthConfirmed { get; set; }
    public string? ShrinkageApproved { get; set; }
    public string? TorqueApproved { get; set; }
    public string? QualityStandardReady { get; set; }
    public string? LinePlanned { get; set; }
    public string? OverallResult { get; set; }
    public string? OpenConditions { get; set; }
    public int? ResponsibleOwnerId { get; set; }
    public string? ResponsibleOwnerName { get; set; }
    public string? ResponsibleOwner => ResponsibleOwnerName;
    public DateTime? DueDate { get; set; }
    public int? ApprovedByUserId { get; set; }
    public string? ApprovedByName { get; set; }
    public string? ApprovedBy => ApprovedByName;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
