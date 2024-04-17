using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Data
{
    public static class FieldRatingModelMapper
    {
        public static IEnumerable<FieldRatingModel> Map(this IEnumerable<FieldRating> model) 
            =>  model.Select(fieldRating => fieldRating.Map());
        

        public static FieldRatingModel Map(this FieldRating model)
        {
            return new FieldRatingModel
            {
                RatingId = model.Id.Value,
                FieldId = model.FieldId.Value,
                Rating = model.Rating,
                Content = model.Content,
                CreatorId = model.CreatedBy
            };
        }

        public static IEnumerable<FieldRating> Map(this IEnumerable<FieldRatingModel> model)
            => model.Select(fieldRating => fieldRating.Map());

        public static FieldRating Map(this FieldRatingModel model)
        {
            return new FieldRating
            {
                Id = model.RatingId is null ? default : new FieldRatingId(model.RatingId.Value),
                FieldId = new FieldId(model.FieldId),
                Rating = model.Rating,
                Content = model.Content,
                CreatedBy = model.CreatorId,
                CreatedOnUtc = DateTime.UtcNow
            };
        }

    }
}
