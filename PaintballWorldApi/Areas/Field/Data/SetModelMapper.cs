using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Data
{
    public static class SetModelMapper
    {
        public static Set Map(this SetDto set, FieldId fieldId)
        {
            return new Set
            {
                Id = SetId.NewSetId(),
                Ammo = set.Ammo,
                Price = set.Price,
                Description = set.Description,
                FieldId = fieldId,
            };
        }

        public static IList<Set> Map(this IList<SetDto> set, FieldId fieldId) => 
            set.Select(setDto => new Set
                {
                    Id = SetId.NewSetId(),
                    Ammo = setDto.Ammo,
                    Price = setDto.Price,
                    Description = setDto.Description,
                    FieldId = fieldId,
                })
                .ToList();
        
    }
}
