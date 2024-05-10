using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Data
{
    public static class SetModelMapper
    {
        // public static Set Map(this SetDto Set, FieldId fieldId)
        // {
        //     return new Set
        //     {
        //         FieldId = Set.FieldId,
        //         Ammo = Set.Ammo,
        //         Price = Set.Price,
        //         Description = Set.Description,
        //         FieldId = fieldId,
        //     };
        // }
        //
        // public static IEnumerable<Set> Map(this IList<SetDto> Set, FieldId fieldId) => 
        //     Set.Select(setDto => new Set
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
