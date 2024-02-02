using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class Address
{
    public int Id { get; set; }

    public string? PhoneNo { get; set; }

    public string? Street { get; set; }

    public string? HouseNo { get; set; }

    public string? City { get; set; }

    public string? PostalNumber { get; set; }

    public string? Country { get; set; }

    public string? Coordinates { get; set; }
}
