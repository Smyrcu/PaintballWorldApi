using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct EmailOutboxId(Guid Value)
{
    public static EmailOutboxId Empty => new(Guid.Empty);
    public static EmailOutboxId NewEventId() => new(Guid.NewGuid());
}

public partial class EmailOutbox
{
    public EmailOutboxId Id { get; private set; } = EmailOutboxId.Empty;
    public string? MessageId { get; set; }

    public string? Recipient { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public DateTime? SentTime { get; set; }

    public bool? IsSent { get; set; }

    public int? SendTries { get; set; }
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
