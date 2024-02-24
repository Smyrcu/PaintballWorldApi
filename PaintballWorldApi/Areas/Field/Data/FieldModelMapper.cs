using Microsoft.CodeAnalysis.CSharp.Syntax;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Data
{
    public static class FieldModelMapper
    {
        public static Infrastructure.Models.Field Map(this FieldDto fieldDto, FieldTypeId fieldTypeId)
        {
            var field = new Infrastructure.Models.Field
            {
                Id = FieldId.NewFieldId(),
                Address = new Address
                {
                    Id = AddressId.NewAddressId(),
                    PhoneNo = fieldDto.Address.PhoneNo,
                    Street = fieldDto.Address.Street,
                    HouseNo = fieldDto.Address.HouseNo,
                    City = fieldDto.Address.City,
                    PostalNumber = fieldDto.Address.PostalNumber,
                    Country = fieldDto.Address.Country,
                    Coordinates = fieldDto.Address.Coordinates
                },
                OwnerId = fieldDto.OwnerId.Value,
                Area = fieldDto.Area,
                Name = fieldDto.Name,
                // Regulations = regPath,
                Description = fieldDto.Description,
                MinPlayers = fieldDto.MinPlayers,
                MaxPlayers = fieldDto.MaxPlayers,
                MaxSimultaneousEvents = fieldDto.MaxSimultaneousEvents,
                FieldTypeId = fieldTypeId
            };
            if (fieldDto.Sets.Any())
            {
                field.Sets = fieldDto.Sets.Map(field.Id);
            }
            

            return field;
        }
    }
}
