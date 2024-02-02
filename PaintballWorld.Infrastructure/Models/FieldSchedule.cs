using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class FieldSchedule
{
    public int Id { get; set; }

    public int FieldId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? Time { get; set; }

    public bool IsRecurrent { get; set; }

    public int? DayOfWeek { get; set; }

    public int? HowManyWeeksActive { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime CreatedUtc { get; set; }
}
