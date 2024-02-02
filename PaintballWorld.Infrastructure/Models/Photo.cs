using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Photo
{
    public int Id { get; set; }

    public int EntityTypeId { get; set; }

    public int EntityId { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
