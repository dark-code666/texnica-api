namespace ERP.Api.Domain;

public class FabricRequirement : BaseEntity
{
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Style y Color se mantienen aquí como texto descriptivo de la especificación técnica
    // de la tela requerida (pueden diferir del FGPO general). No son relaciones.
    public string? Style { get; set; }
    public string? Color { get; set; }

    // Componente de tela normalizado: FK a Components
    public int? ComponentId { get; set; }
    public Component? Component { get; set; }

    public string? FabricDescription { get; set; }
    public string? Composition { get; set; }
    public decimal GSM { get; set; }
    public string? RequiredWidth { get; set; }
    public string? UOM { get; set; }
    public decimal OrderQuantity { get; set; }
    public decimal ApprovedYield { get; set; }
    public decimal GrossRequirement { get; set; }
    public decimal AllowancePercentage { get; set; }
    public decimal AllowanceQty { get; set; }
    public decimal AvailableInventory { get; set; }
    public decimal NetPurchaseRequirement { get; set; }
    public DateTime RequiredDate { get; set; }
    public string? Status { get; set; }

    // Dueño del dato: FK a Users (usuario logueado)
    public int? DataOwnerId { get; set; }
    public User? DataOwner { get; set; }

    public string? Remarks { get; set; }
}
