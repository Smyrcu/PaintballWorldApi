namespace PaintballWorld.API.Areas.Schedule.Models;

public class CreateSchedulesDto
{
    //public List<ScheduleTime> ScheduleDates { get; set; } = [];
    public string? EventType { get; set; } // open/private
    public List<string> SelectedDays { get; set; } = [];
    public DateTime? FinalDate { get; set; }
    public string? Name { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Description { get; set; }
    public int? TimeValue { get; set; }
    public bool IsMultiple { get; set; }
    public bool IsWeekly { get; set; }
    public bool IsAutomatic { get; set; }
    public int MaxPlayers { get; set; }
}
