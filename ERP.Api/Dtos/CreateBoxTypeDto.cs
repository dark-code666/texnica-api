namespace ERP.Api.Dtos;

public class CreateBoxTypeDto
{
    public string BoxCode { get; set; } = null!;
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? EmptyCartonWeight { get; set; }
    public string? Comments { get; set; }
}
