namespace ERP.Api.Dtos;

public class CreateColorDto
{
    public string? ColorCode { get; set; }
    public string? AlternateCode { get; set; }
    public string ColorName { get; set; } = null!;
    public string? DyeMethod { get; set; }
}
