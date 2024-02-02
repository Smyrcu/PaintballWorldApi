using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class FieldType
{
    public int Id { get; set; }

    public string FieldType1 { get; set; } = null!;

    public DateTime? CreatedOnUtc { get; set; }
}
