namespace PaintballWorld.Infrastructure.Models;

public readonly record struct FieldTypeId(Guid Value)
{
    public static FieldTypeId Empty => new(Guid.Empty);
    public static FieldTypeId NewEventId() => new(Guid.NewGuid());
}


public partial class FieldType
{
    public FieldTypeId Id { get; init; }// = FieldTypeId.Empty;
    public string FieldTypeName { get; set; } = null!;
    public virtual ICollection<Field> Fields { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
