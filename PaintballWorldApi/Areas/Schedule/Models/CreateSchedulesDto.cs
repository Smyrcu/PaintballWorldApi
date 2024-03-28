namespace PaintballWorld.API.Areas.Schedule.Models;

public class CreateSchedulesDto
{
    public List<ScheduleTime> ScheduleDates { get; set; } = [];
}


public class ScheduleTime
{
    public DateTime ScheduleDate { get; set; }
    public TimeSpan MaxPlayTime { get; set; }
    public int MaxPlayers { get; set; }
}