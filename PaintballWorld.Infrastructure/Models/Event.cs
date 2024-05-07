using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct EventId(Guid Value)
{
    public static EventId Empty => new(Guid.Empty);
    public static EventId NewEventId() => new(Guid.NewGuid());
}

public partial class Event
{
    public EventId Id { get; init; }
    public FieldId FieldId { get; set; }
    public virtual Field Field { get; set; }
    public string? Description { get; set; }
    public string CreatedBy { get; set; }
    public string Name { get; set; }
    public FieldScheduleId? FieldScheduleId { get; set; }
    public virtual FieldSchedule? FieldSchedule { get; set; }
    public virtual IdentityUser? CreatedByUser { get; set; }
    public int MaxPlayers { get; set; }
    public bool IsPublic { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = [];
    public ICollection<UsersToEvent> UsersToEvents { get; set; } = [];
}
