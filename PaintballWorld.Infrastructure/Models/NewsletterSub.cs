using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class NewsletterSub
{
    public int Id { get; set; }

    public int NewsletterTypeId { get; set; }

    public string Email { get; set; } = null!;
}
