using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class UserRating
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public double Rating { get; set; }

    public string? Content { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
