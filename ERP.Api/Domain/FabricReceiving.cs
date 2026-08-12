namespace ERP.Api.Domain;

public class FabricReceiving : BaseEntity
{
    public string ReceivingNumber { get; set; } = null!;
    public DateTime ReceivingDate { get; set; }
    public string? ShipmentNumber { get; set; }
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Proveedor: derivado de FabricPO.SupplierId → se obtiene por JOIN

    public decimal PackingListQty { get; set; }
    public decimal ActualReceivedQty { get; set; }
    public decimal ReceivingVariance { get; set; }
    public decimal ReceivingShortage { get; set; }
    public decimal ReceivingOverQty { get; set; }
    public int ExpectedRolls { get; set; }
    public int ReceivedRolls { get; set; }
    public int MissingRolls { get; set; }
    public string? ReceivingStatus { get; set; }
    public string? WarehouseLocation { get; set; }

    // Recibido por: FK a Users (usuario logueado)
    public int? ReceivedByUserId { get; set; }
    public User? ReceivedBy { get; set; }

    // Dueño del dato: FK a Users (usuario logueado)
    public int? DataOwnerId { get; set; }
    public User? DataOwner { get; set; }

    public string? Remarks { get; set; }
}
