namespace ERP.Api.Dtos;

public class CuttingControlDto
{
    public int ID { get; set; }
    public DateTime CutDate { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public int? SizeId { get; set; }
    public string? SizeName { get; set; }
    public string? FabricLot { get; set; }
    public string? MarkerNumber { get; set; }
    public int PlannedCut { get; set; }
    public int ActualCut { get; set; }
    public int GoodCut { get; set; }
    public int DamagedQty { get; set; }
    public int ReplacementCut { get; set; }
    public int SentToSewing { get; set; }
    public int CuttingVariance { get; set; }
    public int PendingCut { get; set; }
    public int OvercutQty { get; set; }
    public int CutToSewDifference { get; set; }
    public string? ReleaseStatus { get; set; }
    public int? ResponsiblePersonId { get; set; }
    public string? ResponsiblePersonName { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
