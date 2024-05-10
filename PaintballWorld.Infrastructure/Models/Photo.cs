namespace PaintballWorld.Infrastructure.Models;

public readonly record struct PhotoId(Guid Value)
{
    public static PhotoId Empty => new(Guid.Empty);
    public static PhotoId NewEventId() => new(Guid.NewGuid());
}


public partial class Photo
{
    public PhotoId Id { get; init; }// = PhotoId.Empty;

    public string Path { get; set; }

    // public EntityTypeId EntityTypeId { get; Set; }
    // public virtual EntityType EntityType { get; Set; }
    public FieldId? FieldId { get; set; }
    public EventId? EventId { get; set; }
    public DateTime? CreatedOnUtc { get; set; }

}
