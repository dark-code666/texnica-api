namespace ERP.Api.Domain;

public class FgpoLine : BaseEntity
{
    public int FgpoId { get; set; }
    public Fgpo? Fgpo { get; set; }
    public int StyleId { get; set; }
    public Style? Style { get; set; }
    public int ColorId { get; set; }
    public Color? Color { get; set; }
    public int SizeId { get; set; }
    public Size? Size { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}
