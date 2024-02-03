using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Newsletter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public int TypeId { get; set; }
}
