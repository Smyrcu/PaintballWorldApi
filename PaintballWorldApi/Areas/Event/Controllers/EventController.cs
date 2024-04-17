using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaintballWorld.API.Areas.Event.Controllers
{
    [Route("api/[Area]/[controller]")]
    [Area("Event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPublicEvents()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePublicEvent()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditPublicEvent()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePublicEvent()
        {
            throw new NotImplementedException();
        }
    }
}
