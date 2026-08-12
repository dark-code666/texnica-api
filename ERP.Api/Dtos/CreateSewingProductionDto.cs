namespace ERP.Api.Dtos;

public class CreateSewingProductionDto
{
    public DateTime ProductionDate { get; set; }
    public string? Shift { get; set; }
    public string? Line { get; set; }
    public int FGPOId { get; set; }
    public int? SizeId { get; set; }
    public int SewingInput { get; set; }
    public int DailyTarget { get; set; }
    public int DailyOutput { get; set; }
    public int CumulativeOutput { get; set; }
    public int Wip { get; set; }
    public int Rework { get; set; }
    public int Reject { get; set; }
    public int DowntimeMinutes { get; set; }
    public string? TopStatus { get; set; }
    public int? SupervisorId { get; set; }
    public string? Remarks { get; set; }
}
