namespace PaintballWorld.Infrastructure.Models;

public readonly record struct FieldRatingId(Guid Value)
{
    public static FieldRatingId Empty = new (Guid.Empty);
    public static FieldRatingId NewFieldRatingId() => new (Guid.NewGuid());
}

public partial class FieldRating
{
    public FieldRatingId Id { get; init; } = FieldRatingId.Empty;
    public FieldId FieldId { get; set; }
    public virtual Field Field { get; set; }
    public double Rating { get; set; }

    public string? Content { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
