using PaintballWorld.API.Areas.Rating.Models;
using PaintballWorld.Core.Models;

namespace PaintballWorld.API.Areas.Rating.Data
{
    public static class FieldRatingModelMapper
    {
        public static FieldRatingModel Map(this FieldRatingDto dto, string userId)
        {
            return new FieldRatingModel
            {
                RatingId = dto.RatingId,
                FieldId = dto.FieldId,
                Rating = dto.Rating,
                Content = dto.Content,
                CreatorId = userId,

            };
        }

        public static FieldRatingDto Map(this FieldRatingModel model)
        {
            return new FieldRatingDto
            {
                FieldId = model.FieldId,
                RatingId = model.RatingId.GetValueOrDefault(),
                Rating = model.Rating,
                Content = model.Content,

            };
        }


    }
}
