using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Event.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Schedule.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Schedule")]
    [ApiController]
    public class PublicEventController(ApplicationDbContext context) : ControllerBase
    {

        /// <summary>
        /// Pobierz publiczne eventy
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{fieldGuid:guid}")]
        public async Task<IActionResult> GetPublicEvents([FromRoute]Guid fieldGuid)
        {
            var result = context.Events.Where(x => x.IsPublic && x.FieldId == new FieldId(fieldGuid)).OrderBy(x => x.StartDate).Take(100)
                .Select(x => new PublicEvent
                {
                    Name = x.Name,
                    EventId = x.Id,
                    Date = x.StartDate,
                    SignedPlayers = x.UsersToEvents.Count,
                    MaxPlayers = x.MaxPlayers
                }).ToList();

            return Ok(result);

        }
    }
}
