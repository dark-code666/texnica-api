namespace ERP.Api.Domain;

public class CuttingPanelQc : BaseEntity
{
    public DateTime InspectionDate { get; set; }
    public int FGPOId { get; set; }
    public Fgpo? FGPO { get; set; }

    // Talla normalizada: FK a Sizes
    public int? SizeId { get; set; }
    public Size? Size { get; set; }

    public string? FabricLot { get; set; }
    public string? CutLotLay { get; set; }
    public string? BundleNo { get; set; }
    public int SampleQty { get; set; }
    public int PanelDefects { get; set; }
    public int NotchesDefects { get; set; }
    public int DrillMarkDefects { get; set; }
    public int ShadeDefects { get; set; }
    public int MeasurementDefects { get; set; }
    // Columnas calculadas por SQL (fórmulas del Excel)
    public int TotalDefects { get; set; }
    public decimal DefectRatePct { get; set; }
    public decimal MaxAllowed { get; set; }
    public string? Result { get; set; }

    // Inspector: FK a Users (usuario logueado)
    public int? InspectorId { get; set; }
    public User? Inspector { get; set; }

    public string? CorrectiveAction { get; set; }
    public string? Comments { get; set; }
}
