namespace PaintballWorld.API.Areas.Rating.Models;

public class FieldRatingDto : IRating
{
    public Guid FieldId { get; set; }
    public Guid RatingId { get; set; }
    public double Rating { get; set; }
    public string? Content { get; set; }
}