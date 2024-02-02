using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class FieldRating
{
    public int Id { get; set; }

    public int FieldId { get; set; }

    public double Rating { get; set; }

    public string? Content { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
