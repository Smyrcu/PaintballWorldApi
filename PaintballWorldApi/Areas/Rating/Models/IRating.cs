using Microsoft.AspNetCore.Routing.Constraints;

namespace PaintballWorld.API.Areas.Rating.Models
{
    public interface IRating
    { 
        public Guid RatingId { get; set; }
        public double Rating { get; set; }
        public string? Content { get; set; }
    }
}
