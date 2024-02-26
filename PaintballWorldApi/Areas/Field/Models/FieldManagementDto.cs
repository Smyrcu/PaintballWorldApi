using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Models
{
    public class FieldManagementDto
    {
        public FieldId FieldId { get; init; }
        public AddressDto Address { get; set; }
        public OwnerId? OwnerId { get; set; }
        public FieldTypeId FieldTypeId { get; set; }
        public double Area { get; set; }
        public string Name { get; set; }
        public string? Regulations { get; set; }
        public string? Description { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MaxSimultaneousEvents { get; set; }
        public string FieldType { get; set; }

        public IList<SetDto> Sets { get; set; } = new List<SetDto>();

        
    }
}
