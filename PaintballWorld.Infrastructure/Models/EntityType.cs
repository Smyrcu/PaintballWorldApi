namespace PaintballWorld.Infrastructure.Models;

public readonly record struct EntityTypeId(Guid Value)
{
    public static EntityTypeId Empty => new(Guid.Empty);
    public static EntityTypeId NewEventId() => new(Guid.NewGuid());
}


public partial class EntityType
{
    public EntityTypeId Id { get; init; }// = EntityTypeId.Empty;
    public string? Name { get; set; }
}
