namespace ERP.Api.Domain;

public class Lot : BaseEntity
{
    public string LotNumber { get; set; } = null!;
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    public decimal ProducedQuantity { get; set; }
}
