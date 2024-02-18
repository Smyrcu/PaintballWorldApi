using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct UsersToEventId(Guid Value)
{
    public static UsersToEventId Empty => new(Guid.Empty);
    public static UsersToEventId NewEventId() => new(Guid.NewGuid());
}


public partial class UsersToEvent
{
    public UsersToEventId Id { get; init; } = UsersToEventId.Empty;
    public EventId EventId { get; set; }
    public virtual Event Event { get; set; }

    public SetId? SetId { get; set; }
    public virtual Set? set { get; set; }

    public string UserId { get; set; }
    public virtual IdentityUser User { get; set; }

    public DateTime? JoinedOnUtc { get; set; }
}
