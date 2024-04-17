using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Rating.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Rating.Controllers
{
    [Route("api/[Area]/[controller]")]
    [Area("Rating")]
    [ApiController]
    public class UserRatingController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetFieldRatings()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFieldRating([FromBody] UserRatingDto dto)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditSubmittedRating([FromBody] UserRatingDto dto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IAsyncResult> DeleteRating(UserRatingId id)
        {
            throw new NotImplementedException();
        }
    }
}
