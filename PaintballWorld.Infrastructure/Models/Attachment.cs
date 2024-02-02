using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Attachment
{
    public int Id { get; set; }

    public int EmailId { get; set; }

    public bool EmailType { get; set; }

    public string? Path { get; set; }
}
