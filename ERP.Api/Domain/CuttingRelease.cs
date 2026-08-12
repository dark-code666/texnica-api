namespace ERP.Api.Domain;

public class CuttingRelease : BaseEntity
{
    public string? ReleaseNumber { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    public string? FabricLot { get; set; }
    public decimal ApprovedCutQty { get; set; }
    public decimal ApprovedWidth { get; set; }
    public string? MarkerNumber { get; set; }
    public decimal ApprovedYield { get; set; }
    // Resultado del PRR (dropdown Excel: Ready, Ready with Conditions, Not Ready, Blocked)
    public string? PrrResult { get; set; }

    // Liberado por: FK a Users (usuario logueado)
    public int? ReleasedByUserId { get; set; }
    public User? ReleasedBy { get; set; }

    // Revisado por: FK a Users
    public int? ReviewedByUserId { get; set; }
    public User? ReviewedBy { get; set; }

    public string? Exception { get; set; }
    public string? Conditions { get; set; }
    // Estado de la liberación (dropdown Excel: Pending, Approved, Rejected, On Hold, Cancelled)
    public string? ReleaseStatus { get; set; }
    public string? Comments { get; set; }
}
