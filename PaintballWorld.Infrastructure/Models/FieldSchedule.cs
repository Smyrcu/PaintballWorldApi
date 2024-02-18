namespace PaintballWorld.Infrastructure.Models;
public readonly record struct FieldScheduleId(Guid Value)
{
    public static FieldScheduleId Empty => new(Guid.Empty);
    public static FieldScheduleId NewEventId() => new(Guid.NewGuid());
}


public partial class FieldSchedule
{
    public FieldScheduleId Id { get; init; } = FieldScheduleId.Empty;
    public FieldId FieldId { get; set; }
    public virtual Field Field { get; set; }
    public DateOnly Date { get; set; }

    public TimeOnly? Time { get; set; }

    public bool IsRecurrent { get; set; }

    public int? DayOfWeek { get; set; }

    public int? HowManyWeeksActive { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime CreatedUtc { get; set; }
}
