namespace ERP.Api.Domain;

public class ProductionReadiness : BaseEntity
{
    public DateTime ReviewDate { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    // Checklist PRR (dropdown Excel: Pending, Ready, Not Ready, N/A, Exception Approved)
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
    // Resultado calculado por SQL (fórmula del Excel): Blocked > Not Ready > Ready with Conditions > Ready
    public string? OverallResult { get; set; }
    public string? OpenConditions { get; set; }

    // Responsable del seguimiento: FK a Users
    public int? ResponsibleOwnerId { get; set; }
    public User? ResponsibleOwner { get; set; }

    public DateTime? DueDate { get; set; }

    // Aprobado por: FK a Users
    public int? ApprovedByUserId { get; set; }
    public User? ApprovedBy { get; set; }
}
