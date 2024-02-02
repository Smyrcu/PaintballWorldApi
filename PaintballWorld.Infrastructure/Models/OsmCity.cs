using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class OsmCity
{
    public int Id { get; set; }

    public long? OsmId { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Name { get; set; }

    public string? County { get; set; }

    public string? Municipality { get; set; }

    public string? Province { get; set; }

    public string? PostalCode { get; set; }
}
