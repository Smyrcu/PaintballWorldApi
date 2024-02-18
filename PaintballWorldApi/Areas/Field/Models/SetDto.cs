namespace PaintballWorld.API.Areas.Field.Models
{
    public class SetDto
    {
        public SetDto(int ammo, decimal? price, string? description)
        {
            Ammo = ammo;
            Price = price;
            Description = description;
        }

        public int Ammo { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }
    }
}
