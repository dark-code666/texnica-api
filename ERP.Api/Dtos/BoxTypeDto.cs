namespace ERP.Api.Dtos;

public class BoxTypeDto
{
    public int ID { get; set; }
    public string BoxCode { get; set; } = null!;
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? EmptyCartonWeight { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
