namespace PaintballWorld.API.Areas.Rating.Models
{
    public class UserRatingDto : IRating
    {
        public Guid UserId { get; set; }
        public Guid RatingId { get; set; }
        public double Rating { get; set; }
        public string? Content { get; set; }
    }
}
