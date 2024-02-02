using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Owner
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string? CompanyName { get; set; }

    public int AddressId { get; set; }

    public string? TaxId { get; set; }

    public bool IsApproved { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
