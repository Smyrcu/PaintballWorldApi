using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Company
{
    public int Id { get; set; }

    public int AddressId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string? Email { get; set; }
}
