using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Rating.Data;
using PaintballWorld.API.Areas.Rating.Models;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Rating.Controllers
{
    [Route("api/[Area]/[controller]")]
    [Area("Rating")]
    [ApiController]
    public class FieldRatingController(IRatingService ratingService, IAuthTokenService authTokenService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetFieldRatings([FromQuery] Guid fieldId)
        {
            var result = ratingService.GetFieldRatings(new FieldId(fieldId)).ToList();

            var response = new RatingResponseBase
            {
                IsSuccess = true,
                Errors = [],
                Message = "",
                Id = default,
                AverageRating = result.Average(x => x.Rating),
                Ratings = result.Select(x => x.Map())
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFieldRating([FromBody] FieldRatingDto dto)
        {
            var userId = authTokenService.GetUserId(User.Claims);
            await ratingService.SubmitFieldRating(dto.Map(userId));
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditSubmittedRating([FromBody] FieldRatingDto dto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRating(FieldRatingId id)
        {
            await ratingService.DeleteFieldRating(id);
            return Ok();
        }
    }

 
}
