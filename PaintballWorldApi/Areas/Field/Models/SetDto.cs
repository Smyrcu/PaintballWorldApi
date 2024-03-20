using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Models
{
    public class SetDto
    {
        public SetDto(int ammo, decimal? price, string? description, Guid? setId)
        {
            Id = setId;
            Ammo = ammo;
            Price = price;
            Description = description;
        }

        public Guid? Id { get; set; }
        public int Ammo { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }
    }
}
