using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct NewsletterId(Guid Value)
{
    public static NewsletterId Empty => new(Guid.Empty);
    public static NewsletterId NewEventId() => new(Guid.NewGuid());
}


public partial class Newsletter
{
    public NewsletterId Id { get; private set; } = NewsletterId.Empty;
    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public int TypeId { get; set; }
}
