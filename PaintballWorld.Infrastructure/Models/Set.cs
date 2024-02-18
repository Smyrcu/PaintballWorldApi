namespace PaintballWorld.Infrastructure.Models;

public readonly record struct SetId(Guid Value)
{
    public static SetId Empty => new(Guid.Empty);
    public static SetId NewSetId() => new(Guid.NewGuid());
}


public partial class Set
{
    public SetId Id { get; init; } = SetId.Empty;

    public int Ammo { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }
    public FieldId FieldId { get; set; } 
    public virtual Field Field { get; set; }
    public ICollection<UsersToEvent>? UsersToEvents { get; set; }
}
