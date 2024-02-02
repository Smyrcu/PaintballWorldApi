using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Field
{
    public int Id { get; set; }

    public int FieldTypeId { get; set; }

    public int AddressId { get; set; }

    public Guid OwnerId { get; set; }

    public double Area { get; set; }

    public string? Name { get; set; }

    public string? Regulations { get; set; }

    public string? Description { get; set; }

    public int MinPlayers { get; set; }

    public int MaxPlayers { get; set; }

    public int MaxSimultaneousEvents { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
