namespace ERP.Api.Domain;

public class FourPointInspection : BaseEntity
{
    public DateTime InspectionDate { get; set; }
    public int? ReceivingId { get; set; }
    public FabricReceiving? Receiving { get; set; }
    public string? ReceivingNumber { get; set; }
    public int FabricPOId { get; set; }
    public FabricPO? FabricPO { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public Lot? Lot { get; set; }
    public string? RollNumber { get; set; }
    public decimal Width { get; set; }
    public decimal InspectedLength { get; set; }
    public int Points1 { get; set; }
    public int Points2 { get; set; }
    public int Points3 { get; set; }
    public int Points4 { get; set; }
    // Columnas calculadas por SQL
    public int TotalPoints { get; set; }
    public decimal PointsPer100SqYd { get; set; }
    public decimal MaxAllowed { get; set; }
    public int AcceptedQty { get; set; }
    public int RejectedQty { get; set; }
    public int HoldQty { get; set; }
    public string? Result { get; set; }
    public string? Inspector { get; set; }
    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
}
