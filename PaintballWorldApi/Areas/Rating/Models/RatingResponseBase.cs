using PaintballWorld.API.BaseModels;

namespace PaintballWorld.API.Areas.Rating.Models
{
    public class RatingResponseBase : ResponseBase
    {
        public Guid Id { get; set; } // field/user
        public double? AverageRating { get; set; }
        public IEnumerable<IRating> Ratings { get; set; } = [];
    }
}
