using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Data
{
    public static class SetModelMapper
    {
        // public static Set Map(this SetDto set, FieldId fieldId)
        // {
        //     return new Set
        //     {
        //         FieldId = set.FieldId,
        //         Ammo = set.Ammo,
        //         Price = set.Price,
        //         Description = set.Description,
        //         FieldId = fieldId,
        //     };
        // }
        //
        // public static IEnumerable<Set> Map(this IList<SetDto> set, FieldId fieldId) => 
        //     set.Select(setDto => new Set
        //         {
        //             FieldId = setDto.FieldId,
        //             Ammo = setDto.Ammo,
        //             Price = setDto.Price,
        //             Description = setDto.Description,
        //             FieldId = fieldId,
        //         });

        public static Set Map(this SetDto setDto, FieldId fieldId)
        {
            return new Set
            {
                Id = setDto.Id.HasValue ? new SetId(setDto.Id.Value) : SetId.Empty,
                Ammo = setDto.Ammo,
                Price = setDto.Price,
                Description = setDto.Description,
                FieldId = fieldId,
            };
        }

        public static IEnumerable<Set> Map(this IList<SetDto> setDtos, FieldId fieldId) =>
            setDtos.Select(setDto => new Set
            {
                Id = setDto.Id.HasValue ? new SetId(setDto.Id.Value) : SetId.Empty,
                Ammo = setDto.Ammo,
                Price = setDto.Price,
                Description = setDto.Description,
                FieldId = fieldId,
            });

        public static Set UpdateFromDto(this Set set, SetDto setDto)
        {
            set.Description = setDto.Description;
            set.Ammo = setDto.Ammo;
            set.Price = setDto.Price;
            return set;
        }
    }
}
