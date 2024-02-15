using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct NewsletterSubId(Guid Value)
{
    public static NewsletterSubId Empty => new(Guid.Empty);
    public static NewsletterSubId NewEventId() => new(Guid.NewGuid());
}


public partial class NewsletterSub
{
    public NewsletterSubId Id { get; private set; } = NewsletterSubId.Empty;

    public int NewsletterTypeId { get; set; }

    public string Email { get; set; } = null!;
}
