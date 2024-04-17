using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintballWorld.Core.Models
{
    public class UserRatingModel
    {
        public Guid? RatingId { get; set; }
        public string UserId { get; set; }
        public double Rating { get; set; }
        public string? Content { get; set; }
        public string CreatorId { get; set; }

    }
}
