using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Set
{
    public int Id { get; set; }

    public int FieldId { get; set; }

    public int Ammo { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }
}
