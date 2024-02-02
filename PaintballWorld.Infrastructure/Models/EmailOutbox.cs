using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class EmailOutbox
{
    public int Id { get; set; }

    public string? MessageId { get; set; }

    public string? Recipient { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public DateTime? SentTime { get; set; }

    public bool? IsSent { get; set; }

    public int? SendTries { get; set; }
}
