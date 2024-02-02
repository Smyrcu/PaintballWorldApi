using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class UsersToEvent
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int? SetId { get; set; }

    public DateTime? JoinedOnUtc { get; set; }
}
