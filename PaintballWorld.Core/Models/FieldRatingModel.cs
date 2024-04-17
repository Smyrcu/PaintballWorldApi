using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Models
{
    public class FieldRatingModel
    {
        public Guid? RatingId { get; set; }
        public Guid FieldId { get; set; }
        public double Rating { get; set; }
        public string? Content { get; set; }
        public string? CreatorId { get; set; }
    }
}
