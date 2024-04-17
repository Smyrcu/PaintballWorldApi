using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaintballWorld.API.Areas.Schedule.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Schedule")]
    [AllowAnonymous]
    public class ReservationController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditReservation()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReservation()
        {
            throw new NotImplementedException();
        }
    }
}
