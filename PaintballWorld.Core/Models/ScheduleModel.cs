using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Models;

public class ScheduleModel
{
    public FieldId FieldId { get; set; }
    public List<ScheduleItem> Schedules { get; set; }
    
}

public class ScheduleItem
{
    public DateTime DateTime { get; set; }
    public FieldScheduleId? FieldScheduleId { get; set; }
    public int MaxPlayers { get; set; }
    public TimeSpan MaxPlayTime { get; set; }
}