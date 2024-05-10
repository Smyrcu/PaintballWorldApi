using Microsoft.EntityFrameworkCore.Design;
using PaintballWorld.Core.Data;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserRatingModel> GetUserRatings(Guid userId)
            => _context.UserRatings.Where(x => x.UserId == userId.ToString()).AsEnumerable().Map();
        

        public IEnumerable<FieldRatingModel> GetFieldRatings(FieldId fieldId) 
            => _context.FieldRatings.Where(x => x.FieldId == fieldId).AsEnumerable().Map();

        public async Task SubmitUserRating(UserRatingModel model)
        {
            UserRating ur = new()
            {
                UserId = model.UserId,
                Rating = model.Rating,
                Content = model.Content,
                CreatorId = model.CreatorId,
                CreatedOnUtc = DateTime.UtcNow
            };
            _context.UserRatings.Add(ur);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitFieldRating(FieldRatingModel model)
        {
            var rating = _context.FieldRatings.FirstOrDefault(x =>
                x.CreatedBy == model.CreatorId && x.FieldId == new FieldId(model.FieldId));

            if (rating == null)
            {
                FieldRating fr = new()
                {
                    FieldId = new FieldId(model.FieldId),
                    Rating = model.Rating,
                    Content = model.Content,
                    CreatedBy = model.CreatorId,
                    CreatedOnUtc = DateTime.UtcNow
                };
                _context.FieldRatings.Add(fr);
                
            }
            else
            {
                rating.Rating = model.Rating;
                rating.Content = model.Content;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserRating(UserRatingId id)
        {
            var rating = _context.UserRatings.FirstOrDefault(x => x.Id == id);
            if (rating != null)
            {
                _context.UserRatings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteFieldRating(FieldRatingId id)
        {
            var rating = _context.FieldRatings.FirstOrDefault(x => x.Id == id);
            if (rating != null)
            {
                _context.FieldRatings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserRating(UserRatingModel model)
        {
            if (model.RatingId is null)
            {
                throw new Exception("Rating Id not found");
            }
            var rating = _context.UserRatings.FirstOrDefault(x => x.Id == new UserRatingId(model.RatingId.Value));

            if (rating is null)
            {
                throw new Exception("Rating not found");
            }

            rating.Content = model.Content;
            rating.Rating = model.Rating;

            _context.UserRatings.Update(rating);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFieldRating(FieldRatingModel model)
        {
            if (model.RatingId is null)
            {
                throw new Exception("Rating Id not found");
            }
            var rating = _context.UserRatings.FirstOrDefault(x => x.Id == new UserRatingId(model.RatingId.Value));

            if (rating is null)
            {
                throw new Exception("Rating not found");
            }

            rating.Content = model.Content;
            rating.Rating = model.Rating;

            _context.UserRatings.Update(rating);
            await _context.SaveChangesAsync();
        }


    }
}
