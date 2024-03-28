using NetTopologySuite.Geometries;
using PaintballWorld.Infrastructure.Models;


namespace PaintballWorld.API.Areas.Auth.Models
{
    public class AddressDto
    {
        public AddressDto(string? PhoneNo, string? Street, string? HouseNo, string? City, string? PostalNumber,
            string? Country, GeoPoint? Location)
        {
            this.PhoneNo = PhoneNo;
            this.Street = Street;
            this.HouseNo = HouseNo;
            this.City = City;
            this.PostalNumber = PostalNumber;
            this.Country = Country;
            this.Location = Location;
        }

        public AddressDto()
        {
            
        }

        public string? PhoneNo { get; init; }
        public string? Street { get; init; }
        public string? HouseNo { get; init; }
        public string? City { get; init; }
        public string? PostalNumber { get; init; }
        public string? Country { get; init; }
        public GeoPoint? Location { get; init; }

        public void Deconstruct(out string? PhoneNo, out string? Street, out string? HouseNo, out string? City, out string? PostalNumber, out string? Country, out GeoPoint? Location)
        {
            PhoneNo = this.PhoneNo;
            Street = this.Street;
            HouseNo = this.HouseNo;
            City = this.City;
            PostalNumber = this.PostalNumber;
            Country = this.Country;
            Location = this.Location;
        }
    }
}
