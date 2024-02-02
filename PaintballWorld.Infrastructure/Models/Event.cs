using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Event
{
    public int Id { get; set; }

    public int FieldTypeId { get; set; }

    public string? Description { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? Time { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
