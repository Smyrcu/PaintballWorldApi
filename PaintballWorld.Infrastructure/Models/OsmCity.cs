using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct OsmCityId(Guid Value)
{
    public static OsmCityId Empty => new(Guid.Empty);
    public static OsmCityId NewEventId() => new(Guid.NewGuid());
}


public partial class OsmCity
{
    public OsmCityId Id { get; private set; } = OsmCityId.Empty;
    public long? OsmId { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Name { get; set; }

    public string? County { get; set; }

    public string? Municipality { get; set; }

    public string? Province { get; set; }

    public string? PostalCode { get; set; }
}
