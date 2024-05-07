using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.API.Areas.Event.Models;
using PaintballWorld.API.Areas.Schedule.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Schedule.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Schedule")]
    [ApiController]
    public class PrivateEventController(ApplicationDbContext context) : ControllerBase
    {
        /// <summary>
        /// Pobierz prywatne(schedule) eventy
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{fieldGuid:guid}")]
        public async Task<IActionResult> GetPrivateEvents([FromRoute]Guid fieldGuid)
        {
            var result = context.FieldSchedules.Where(x => x.FieldId == new FieldId(fieldGuid)).OrderBy(x => x.Date).Take(100)
                .Select(x => new PrivateEvent()
                {
                    Date = x.Date,
                    MaxPlayers = x.MaxPlayers,
                    FieldScheduleId = x.Id.Value,
                    MaxPlaytime = x.MaxPlaytime,
                }).ToList();

            return Ok(result);

        }

        /// <summary>
        /// Pobierz najbliższe wolne terminy
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("open/{fieldGuid:guid}")]
        public async Task<IActionResult> GetOpenPrivateEvents([FromRoute] Guid fieldGuid)
        {
            var result = context.FieldSchedules.Include(x => x.Event)
                .Where(x => x.FieldId == new FieldId(fieldGuid) && x.EventId == null ).OrderBy(x => x.Date).Take(100)
                .Select(x => new PrivateEvent()
                {
                    Date = x.Date,
                    MaxPlayers = x.MaxPlayers,
                    FieldScheduleId = x.Id.Value,
                    MaxPlaytime = x.MaxPlaytime,
                }).ToList();

            return Ok(result);

        }
    }
}
