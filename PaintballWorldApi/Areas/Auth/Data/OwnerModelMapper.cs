using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Auth.Data
{
    public static class OwnerModelMapper
    {
        public static Infrastructure.Models.Owner Map(this OwnerDto ownerDto, IdentityUser user)
        {
            return new Infrastructure.Models.Owner
            {
                UserId = Guid.Parse(user.Id),
                IsApproved = false,
                Company = new Company
                {
                    TaxId = ownerDto.Company.TaxId,
                    CompanyName = ownerDto.Company.CompanyName,
                    Email = ownerDto.Company.Email,
                    Address = new Address
                    {
                        PhoneNo = ownerDto.Company.Address.PhoneNo,
                        Street = ownerDto.Company.Address.Street,
                        HouseNo = ownerDto.Company.Address.HouseNo,
                        City = ownerDto.Company.Address.City,
                        PostalNumber = ownerDto.Company.Address.PostalNumber,
                        Country = ownerDto.Company.Address.Country,
                        Location =
                            ownerDto.Company.Address.Location is not null
                                ? new Point(ownerDto.Company.Address.Location.Longitude, ownerDto.Company.Address.Location.Latitude) { SRID = 4326 }
                                : null
                    }
                }
            };
        }



    }
}
