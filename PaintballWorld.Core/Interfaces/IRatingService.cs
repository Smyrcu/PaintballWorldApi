using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IRatingService
{
    IEnumerable<UserRatingModel> GetUserRatings(Guid userId);
    IEnumerable<FieldRatingModel> GetFieldRatings(FieldId fieldId);
    Task SubmitUserRating(UserRatingModel model);
    Task SubmitFieldRating(FieldRatingModel model);
    Task DeleteUserRating(UserRatingId id);
    Task DeleteFieldRating(FieldRatingId id);
    Task UpdateUserRating(UserRatingModel model);
    Task UpdateFieldRating(FieldRatingModel model);
}