namespace ERP.Api.Dtos;

public class StyleYieldDto
{
    public int ID { get; set; }
    public int StyleId { get; set; }
    public string StyleCode { get; set; } = null!;
    public int ComponentId { get; set; }
    public string ComponentCode { get; set; } = null!;
    public decimal? YieldQuoted { get; set; }
    public decimal? YieldReal { get; set; }
    public string? Notes { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
