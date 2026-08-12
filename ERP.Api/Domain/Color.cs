namespace ERP.Api.Domain;

public class Color : BaseEntity
{
    public string ColorName { get; set; } = null!;
    public string? DyeMethod { get; set; }
}
