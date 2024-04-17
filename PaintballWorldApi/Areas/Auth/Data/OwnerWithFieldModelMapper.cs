using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.API.Areas.Field.Data;
using PaintballWorld.API.Areas.User.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Auth.Data
{
    public static class OwnerWithFieldModelMapper
    {
        public static (Owner, Infrastructure.Models.Field) Map(this OwnerWithFieldDto ownerWithFieldDto, IdentityUser user, FieldTypeId FieldTypeId)
        {
            var owner = new Owner
            {
                Id = OwnerId.NewOwnerId(),
                UserId = Guid.Parse(user.Id),
                IsApproved = false,
                Company = new Company
                {
                    Id = CompanyId.NewCompanyId(),
                    TaxId = ownerWithFieldDto.Company.TaxId,
                    CompanyName = ownerWithFieldDto.Company.CompanyName,
                    Email = ownerWithFieldDto.Company.Email,
                    Address = new Address
                    {
                        Id = AddressId.NewAddressId(),
                        PhoneNo = ownerWithFieldDto.Company.Address.PhoneNo,
                        Street = ownerWithFieldDto.Company.Address.Street,
                        HouseNo = ownerWithFieldDto.Company.Address.HouseNo,
                        City = ownerWithFieldDto.Company.Address.City,
                        PostalNumber = ownerWithFieldDto.Company.Address.PostalNumber,
                        Country = ownerWithFieldDto.Company.Address.Country,
                        Location = new Point(ownerWithFieldDto.Company.Address.Location.Longitude, ownerWithFieldDto.Company.Address.Location.Latitude) { SRID = 4326 }
                    }
                }
            };
            ownerWithFieldDto.Field.OwnerId = owner.Id;

            var field = ownerWithFieldDto.Field.Map(FieldTypeId);

            // var field = new Infrastructure.Models.Field
            // {
            //     FieldId = FieldId.NewCompanyId(),
            //     Address = new Address
            //     {
            //         FieldId = AddressId.NewCompanyId(),
            //         PhoneNo = ownerWithFieldDto.Field.Address.PhoneNo,
            //         Street = ownerWithFieldDto.Field.Address.Street,
            //         HouseNo = ownerWithFieldDto.Field.Address.HouseNo,
            //         City = ownerWithFieldDto.Field.Address.City,
            //         PostalNumber = ownerWithFieldDto.Field.Address.PostalNumber,
            //         Country = ownerWithFieldDto.Field.Address.Country,
            //         Coordinates = ownerWithFieldDto.Field.Address.Coordinates
            //     },
            //     OwnerId = owner.FieldId,
            //     Area = ownerWithFieldDto.Field.Area,
            //     Name = ownerWithFieldDto.Field.Name,
            //     Regulations = ownerWithFieldDto.Field.Regulations,
            //     Description = ownerWithFieldDto.Field.Description,
            //     MinPlayers = ownerWithFieldDto.Field.MinPlayers,
            //     MaxPlayers = ownerWithFieldDto.Field.MaxPlayers,
            //     MaxSimultaneousEvents = ownerWithFieldDto.Field.MaxSimultaneousEvents,
            // };
            return (owner, field);

        }
    }
}
