using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.API.Areas.User.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;

namespace PaintballWorld.API.Areas.User.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("User")]
    [ApiController]
    public class UserHistoryController(ApplicationDbContext context, IAuthTokenService authTokenService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetMyHistory()
        {
            var userId = authTokenService.GetUserId(User.Claims);

            var history = context.UsersToEvents
                .Include(x => x.Event)
                .ThenInclude(x => x.Field)
                .ThenInclude(y => y.Sets)
                .Where(x => x.UserId == userId && x.Event.EndDate < DateTime.UtcNow)
                .Select(x => new Game
                {
                    Field = new Models.Field
                    {
                        FieldId = x.Event.Field.Id.Value,
                        FieldName = x.Event.Field.Name,
                        City = x.Event.Field.Address.City
                    },
                    Event = new Models.Event
                    {
                        EventId = x.Event.Id.Value,
                        Ammo = x.set.Ammo,
                        Price = x.set.Price,
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
