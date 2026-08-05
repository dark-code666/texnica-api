namespace ERP.Api.Domain;

/// <summary>
/// Catálogo maestro normalizado: UOMs, Fabric Components, Statuses, Test Results, etc.
/// Reemplaza los arrays estáticos duplicados en los servicios.
/// </summary>
public class CatalogValue : BaseEntity
{
    public string Type { get; set; } = null!; // e.g. "UOM", "FabricComponent", "ProductionStatus"
    public string Value { get; set; } = null!;
}
