using Microsoft.AspNetCore.Identity;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Auth.Data
{
    public static class OwnerModelMapper
    {
        public static Owner Map(this OwnerDto ownerDto, IdentityUser user)
        {
            return new Owner
            {
                Id = OwnerId.NewOwnerId(),
                UserId = Guid.Parse(user.Id),
                IsApproved = false,
                Company = new Company
                {
                    Id = CompanyId.NewCompanyId(),
                    TaxId = ownerDto.Company.TaxId,
                    CompanyName = ownerDto.Company.CompanyName,
                    Email = ownerDto.Company.Email,
                    Address = new Address
                    {
                        Id = AddressId.NewAddressId(),
                        PhoneNo = ownerDto.Company.Address.PhoneNo,
                        Street = ownerDto.Company.Address.Street,
                        HouseNo = ownerDto.Company.Address.HouseNo,
                        City = ownerDto.Company.Address.City,
                        PostalNumber = ownerDto.Company.Address.PostalNumber,
                        Country = ownerDto.Company.Address.Country,
                        Coordinates = ownerDto.Company.Address.Coordinates
                    }
                }
            };
        }



    }
}
