namespace ERP.Api.Domain;

public class Price : BaseEntity
{
    public int StyleId { get; set; }
    public Style? Style { get; set; }
    public int ColorId { get; set; }
    public Color? Color { get; set; }
    public int SizeId { get; set; }
    public Size? Size { get; set; }
    public string? Sku { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Comments { get; set; }
}
