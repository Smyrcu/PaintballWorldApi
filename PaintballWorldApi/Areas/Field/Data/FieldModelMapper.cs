using System.Collections.ObjectModel;
using System.Transactions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NetTopologySuite.Geometries;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Data
{
    public static class FieldModelMapper
    {
        public static Infrastructure.Models.Field Map(this FieldDto fieldDto, FieldTypeId fieldTypeId)
        {
            var field = new Infrastructure.Models.Field
            {
                Address = new Address
                {
                    PhoneNo = fieldDto.Address.PhoneNo,
                    Street = fieldDto.Address.Street,
                    HouseNo = fieldDto.Address.HouseNo,
                    City = fieldDto.Address.City,
                    PostalNumber = fieldDto.Address.PostalNumber,
                    Country = fieldDto.Address.Country,
                    Location = new Point(fieldDto.Address.Location.Longitude, fieldDto.Address.Location.Latitude)
                        { SRID = 4326 }
                },
                OwnerId = fieldDto.OwnerId.Value,
                Area = fieldDto.Area,
                Name = fieldDto.Name,
                Regulations = fieldDto.RegulationsPath,
                // Regulations = regPath,
                Description = fieldDto.Description,
                MinPlayers = fieldDto.MinPlayers,
                MaxPlayers = fieldDto.MaxPlayers,
                MaxSimultaneousEvents = fieldDto.MaxSimultaneousEvents,
                FieldTypeId = fieldTypeId
            };
            if (fieldDto.Sets.Any())
            {
                field.Sets = fieldDto.Sets.Map(field.Id).ToList();
            }


            return field;
        }

        public static FieldManagementDto Map(this Infrastructure.Models.Field field, string urlPrefix)
        {
            var result = new FieldManagementDto()
            {
                FieldId = field.Id,
                Address = field.Address.Map(),
                OwnerId = field.OwnerId,
                Area = field.Area,
                Name = field.Name ?? string.Empty,
                Regulations = $"{urlPrefix}/{field.Regulations}",
                Description = field.Description,
                MinPlayers = field.MinPlayers,
                MaxPlayers = field.MaxPlayers,
                MaxSimultaneousEvents = field.MaxSimultaneousEvents,
                FieldType = field.FieldType.FieldTypeName,
                Sets = field.Sets.Map()
            };

            return result;
        }

        public static IList<SetDto> Map(this ICollection<Set> sets) =>
            sets.Select(set => new SetDto(set.Ammo, set.Price, set.Description, set.Id.Value)).ToList();


        public static AddressDto Map(this Address address)
        {
            var result = new AddressDto
            {
                PhoneNo = address.PhoneNo,
                Street = address.Street,
                HouseNo = address.HouseNo,
                City = address.City,
                PostalNumber = address.PostalNumber,
                Country = address.Country,
                Location = address.Location is not null
                    ? new GeoPoint(address.Location.X, address.Location.Y)
                    : null

            };
            return result;
        }

        public static Address Map(this AddressDto address)
        {
            var result = new Address
            {
                PhoneNo = address.PhoneNo,
                Street = address.Street,
                HouseNo = address.HouseNo,
                City = address.City,
                PostalNumber = address.PostalNumber,
                Country = address.Country,
                Location = address.Location is not null
                    ? new Point(address.Location.Longitude, address.Location.Latitude) { SRID = 4326 }
                    : null
            };
            return result;
        }

        public static Infrastructure.Models.Field Map(this FieldManagementDto dto)
        {
            var result = new Infrastructure.Models.Field
            {
                Id = dto.FieldId,
                FieldTypeId = dto.FieldTypeId,
                Address = dto.Address.Map(),
                Area = dto.Area,
                Name = dto.Name,
                Description = dto.Description,
                MinPlayers = dto.MinPlayers,
                MaxPlayers = dto.MaxPlayers,
                Sets = dto.Sets.Map(dto.FieldId).ToList(),
                MaxSimultaneousEvents = dto.MaxSimultaneousEvents
            };
            return result;
        }

        public static FilteredFieldDto Map(this FilteredField model)
        {
            return new FilteredFieldDto
            {
                FieldId = model.FieldId.Value,
                FieldName = model.FieldName,
                CityName = model.CityName,
                GeoPoint = new Point(model.GeoPoint.X, model.GeoPoint.Y)
            };

        }

        public static IEnumerable<FilteredFieldDto> Map(this IEnumerable<FilteredField> model)
            => model.Select(x => x.Map());


    }
}

    
