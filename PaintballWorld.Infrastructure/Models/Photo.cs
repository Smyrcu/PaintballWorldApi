using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct PhotoId(Guid Value)
{
    public static PhotoId Empty => new(Guid.Empty);
    public static PhotoId NewEventId() => new(Guid.NewGuid());
}


public partial class Photo
{
    public PhotoId Id { get; private set; } = PhotoId.Empty;
    public EntityTypeId EntityTypeId { get; set; }
    public virtual EntityType EntityType { get; set; }
    public FieldId? FieldId { get; set; }
    public EventId? EventId { get; set; }
    public DateTime? CreatedOnUtc { get; set; }

}
