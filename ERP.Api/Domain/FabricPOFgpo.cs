namespace ERP.Api.Domain;

public class FabricPOFgpo
{
    public int ID { get; set; }
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Style y Color viven aquí porque cada FGPO cubierto por el mismo Fabric PO
    // puede tener su propio estilo/color.
    public string? Style { get; set; }
    public string? Color { get; set; }

    // Cantidad de esta orden de compra asignada específicamente a este FGPO.
    public decimal AllocatedQuantity { get; set; }

    public DateTime? LastUpdated { get; set; }
}
