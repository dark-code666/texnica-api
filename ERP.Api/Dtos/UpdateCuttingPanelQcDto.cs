namespace ERP.Api.Dtos;

public class UpdateCuttingPanelQcDto
{
    public DateTime InspectionDate { get; set; }
    public int FGPOId { get; set; }
    public int? SizeId { get; set; }
    public string? FabricLot { get; set; }
    public string? CutLotLay { get; set; }
    public string? BundleNo { get; set; }
    public int SampleQty { get; set; }
    public int PanelDefects { get; set; }
    public int NotchesDefects { get; set; }
    public int DrillMarkDefects { get; set; }
    public int ShadeDefects { get; set; }
    public int MeasurementDefects { get; set; }
    public int? InspectorId { get; set; }
    public string? CorrectiveAction { get; set; }
    public string? Comments { get; set; }
}
