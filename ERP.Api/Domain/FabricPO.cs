namespace ERP.Api.Domain;

public class FabricPO : BaseEntity
{
    public string FabricPONumber { get; set; } = null!;

    // Proveedor normalizado: FK a Suppliers
    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public string? FabricMill { get; set; }

    // Componente de tela normalizado: FK a Components
    public int? ComponentId { get; set; }
    public Component? Component { get; set; }

    public decimal OrderedQuantity { get; set; }
    public string? UOM { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal POAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredCompletion { get; set; }
    public DateTime? PlannedExport { get; set; }
    public DateTime? PlannedArrival { get; set; }
    public string? POStatus { get; set; }

    // Responsable de compra: FK a Users (usuario logueado al crear)
    public int? PurchaseOwnerUserId { get; set; }
    public User? PurchaseOwner { get; set; }

    // Aprobado por: FK a Users
    public int? ApprovedByUserId { get; set; }
    public User? ApprovedBy { get; set; }

    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }

    // Relación muchos-a-muchos con FGPO (un Fabric PO puede cubrir varios FGPO)
    public ICollection<FabricPOFgpo> FabricPOFgpos { get; set; } = new List<FabricPOFgpo>();
}
