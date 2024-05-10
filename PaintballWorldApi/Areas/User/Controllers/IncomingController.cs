using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.API.Areas.User.Models;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure;

namespace PaintballWorld.API.Areas.User.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("User")]
    [ApiController]
    public class IncomingController(ApplicationDbContext context, IAuthTokenService authTokenService) : ControllerBase
    {

        /// <summary>
        /// Nadchodzące wydarzenia
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetIncomingEvents()
        {
            var userId = authTokenService.GetUserId(User.Claims);

            var history = context.UsersToEvents
                .Include(x => x.Event)
                .ThenInclude(e => e.Field)
                .ThenInclude(f => f.Address) 
                .Include(x => x.Event.Field)
                .ThenInclude(f => f.Sets) 
                .Where(x => x.UserId == userId && x.Event.EndDate > DateTime.UtcNow)
                .Select(x => new Game
                {
                    IsPublic = x.Event.FieldSchedule == null,
                    Field = new Models.Field
                    {
                        FieldId = x.Event.Field.Id.Value,
                        FieldName = x.Event.Field.Name,
                        City = x.Event.Field.Address.City
                    },
                    Event = new Models.Event
                    {
                        EventId = x.Event.Id.Value,
                        Ammo = x.Set.Ammo,
                        Price = x.Set.Price,
                        Date = x.Event.StartDate
                    }
                })
                .ToList();

            var historyModel = new HistoryModel
            {
                GamesPlayed = history.Count,
                games = history
            };

            return Ok(historyModel);

        }
    }
}
