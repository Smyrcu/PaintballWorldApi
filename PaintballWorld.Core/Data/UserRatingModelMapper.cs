using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintballWorld.Core.Data
{
    public static class UserRatingModelMapper
    {
        public static IEnumerable<UserRatingModel> Map(this IEnumerable<UserRating> model)
            => model.Select(fieldRating => fieldRating.Map());


        public static UserRatingModel Map(this UserRating model)
        {
            return new UserRatingModel
            {
                RatingId = model.Id.Value,
                UserId = model.UserId,
                Rating = model.Rating,
                Content = model.Content,
                CreatorId = model.CreatorId
            };
        }

        public static IEnumerable<UserRating> Map(this IEnumerable<UserRatingModel> model)
            => model.Select(fieldRating => fieldRating.Map());

        public static UserRating Map(this UserRatingModel model)
        {
            return new UserRating
            {
                Id = model.RatingId is null ? default : new UserRatingId(model.RatingId.Value),
                UserId = model.UserId,
                Rating = model.Rating,
                Content = model.Content,
                CreatorId = model.CreatorId,
                CreatedOnUtc = DateTime.UtcNow
            };
        }
    }
}
