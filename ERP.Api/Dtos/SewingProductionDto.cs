namespace ERP.Api.Dtos;

public class SewingProductionDto
{
    public int ID { get; set; }
    public DateTime ProductionDate { get; set; }
    public string? Shift { get; set; }
    public string? Line { get; set; }
    public int FGPOId { get; set; }
    public string? FgpoNumber { get; set; }
    public int? SizeId { get; set; }
    public string? SizeCode { get; set; }
    public int SewingInput { get; set; }
    public int DailyTarget { get; set; }
    public int DailyOutput { get; set; }
    public int CumulativeOutput { get; set; }
    public int Wip { get; set; }
    public int Rework { get; set; }
    public int Reject { get; set; }
    public int DowntimeMinutes { get; set; }
    public decimal TargetAchievementPct { get; set; }
    public int SewingVariance { get; set; }
    public int PendingSewing { get; set; }
    public int Overproduction { get; set; }
    public string? TopStatus { get; set; }
    public int? SupervisorId { get; set; }
    public string? SupervisorName { get; set; }
    public string? Supervisor => SupervisorName;
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
