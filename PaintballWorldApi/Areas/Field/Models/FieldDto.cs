using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas;
public class FieldDto
{
    public AddressDto Address { get; set; }
    public OwnerId? OwnerId { get; set; }
    public double Area { get; set; }
    public string Name { get; set; }
    public string? Regulations { get; set; }
    public string? Description { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int MaxSimultaneousEvents { get; set; }
    public string FieldType { get; set; }

    public IList<SetDto> Sets { get; set; } = new List<SetDto>();

    public FieldDto()
    {
        
    }
    public FieldDto(AddressDto address, OwnerId? ownerId, double area, string name, string? regulations, string? description, int? minPlayers, int? maxPlayers, int? maxSimultaneousEvents, string fieldType)
    {
        Address = address;
        OwnerId = ownerId;
        Area = area;
        Name = name;
        Regulations = regulations;
        Description = description;
        MinPlayers = minPlayers ?? 10;
        MaxPlayers = maxPlayers ?? 100;
        MaxSimultaneousEvents = maxSimultaneousEvents ?? 1;
        FieldType = fieldType;
    }
}