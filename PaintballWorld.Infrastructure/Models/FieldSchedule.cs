namespace PaintballWorld.Infrastructure.Models;
public readonly record struct FieldScheduleId(Guid Value)
{
    public static FieldScheduleId Empty => new(Guid.Empty);
    public static FieldScheduleId NewEventId() => new(Guid.NewGuid());
}


public partial class FieldSchedule
{
    public FieldScheduleId Id { get; init; }
    public FieldId FieldId { get; set; }
    public virtual Field Field { get; set; }
    public EventId? EventId { get; set; }
    public virtual Event? Event { get; set; }
    
    public DateTime Date { get; set; }

    public bool IsRecurrent { get; set; }

    public int? DayOfWeek { get; set; }

    public int MaxPlayers { get; set; }

    public TimeSpan MaxPlaytime { get; set; }

    public int? HowManyWeeksActive { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime CreatedUtc { get; set; }
}
