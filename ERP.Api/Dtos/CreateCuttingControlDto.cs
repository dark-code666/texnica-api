namespace ERP.Api.Dtos;

public class CreateCuttingControlDto
{
    public DateTime CutDate { get; set; }
    public int FGPOId { get; set; }
    public int? SizeId { get; set; }
    public string? FabricLot { get; set; }
    public string? MarkerNumber { get; set; }
    public int PlannedCut { get; set; }
    public int ActualCut { get; set; }
    public int GoodCut { get; set; }
    public int DamagedQty { get; set; }
    public int ReplacementCut { get; set; }
    public int SentToSewing { get; set; }
    public string? ReleaseStatus { get; set; }
    public int? ResponsiblePersonId { get; set; }
    public string? Comments { get; set; }
}
