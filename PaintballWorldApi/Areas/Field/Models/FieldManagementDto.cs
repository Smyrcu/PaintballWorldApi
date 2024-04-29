using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Models
{
    public class FieldManagementDto
    {
        public Guid FieldId { get; init; }
        public AddressDto Address { get; set; }
        public Guid? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public Guid FieldTypeId { get; set; }
        public double Area { get; set; }
        public string Name { get; set; }
        public string? Regulations { get; set; }
        public string? Description { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MaxSimultaneousEvents { get; set; }
        public string FieldType { get; set; }

        public IList<SetDto>? Sets { get; set; } = [];

        
    }
}
