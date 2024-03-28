using NetTopologySuite.Geometries;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct AddressId(Guid Value)
{
    public static AddressId Empty => new(Guid.Empty);
    public static AddressId NewAddressId() => new(Guid.NewGuid());
}

public partial class Address
{
    public AddressId Id { get; init; }
    public string? PhoneNo { get; set; }

    public string? Street { get; set; }

    public string? HouseNo { get; set; }

    public string? City { get; set; }

    public string? PostalNumber { get; set; }

    public string? Country { get; set; }

    public Point? Location { get; set; }

}
