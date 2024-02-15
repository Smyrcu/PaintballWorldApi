using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct AddressId(Guid Value)
{
    public static AddressId Empty => new(Guid.Empty);
    public static AddressId NewFieldId() => new(Guid.NewGuid());
}

public partial class Address
{
    public AddressId Id { get; private set; } = AddressId.Empty;
    public string? PhoneNo { get; set; }

    public string? Street { get; set; }

    public string? HouseNo { get; set; }

    public string? City { get; set; }

    public string? PostalNumber { get; set; }

    public string? Country { get; set; }

    public string? Coordinates { get; set; }
}
