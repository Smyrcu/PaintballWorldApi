using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaintballWorld.API.Areas.Event.Controllers
{
    [Route("api/[Area]/[controller]")]
    [Area("Event")]
    [ApiController]
    public class PublicEventController : ControllerBase
    {
        /// <summary>
        /// Pobierz publiczne eventy
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<IActionResult> GetPublicEvents()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stwórz event
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<IActionResult> CreatePublicEvent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// edytuj event
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPut]
        public async Task<IActionResult> EditPublicEvent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// usuń event
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpDelete]
        public async Task<IActionResult> DeletePublicEvent()
        {
            throw new NotImplementedException();
        }
    }
}
