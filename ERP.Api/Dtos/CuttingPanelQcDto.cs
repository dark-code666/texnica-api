namespace ERP.Api.Dtos;

public class CuttingPanelQcDto
{
    public int ID { get; set; }
    public DateTime InspectionDate { get; set; }
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public int? SizeId { get; set; }
    public string? SizeName { get; set; }
    public string? FabricLot { get; set; }
    public string? CutLotLay { get; set; }
    public string? BundleNo { get; set; }
    public int SampleQty { get; set; }
    public int PanelDefects { get; set; }
    public int NotchesDefects { get; set; }
    public int DrillMarkDefects { get; set; }
    public int ShadeDefects { get; set; }
    public int MeasurementDefects { get; set; }
    public int TotalDefects { get; set; }
    public decimal DefectRatePct { get; set; }
    public decimal MaxAllowed { get; set; }
    public string? Result { get; set; }
    public int? InspectorId { get; set; }
    public string? InspectorName { get; set; }
    public string? CorrectiveAction { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
