namespace ERP.Api.Domain;

public class StyleYield : BaseEntity
{
    public int StyleId { get; set; }
    public Style? Style { get; set; }
    public int ComponentId { get; set; }
    public Component? Component { get; set; }
    public decimal? YieldQuoted { get; set; }
    public decimal? YieldReal { get; set; }
    public string? Notes { get; set; }
}
