using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class EmailInbox
{
    public int Id { get; set; }

    public string? MessageId { get; set; }

    public string? Sender { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public DateTime? ReceivedTime { get; set; }

    public bool? IsRead { get; set; }
}
