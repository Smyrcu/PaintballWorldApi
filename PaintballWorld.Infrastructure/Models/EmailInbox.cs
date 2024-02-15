﻿using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct EmailInboxId(Guid Value)
{
    public static EmailInboxId Empty => new(Guid.Empty);
    public static EmailInboxId NewEventId() => new(Guid.NewGuid());
}

public partial class EmailInbox
{
    public EmailInboxId Id { get; private set; } = EmailInboxId.Empty;
    public string? MessageId { get; set; }

    public string? Sender { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public DateTime? ReceivedTime { get; set; }

    public bool? IsRead { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

}
