using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.API.Areas.Event.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;
using EventId = PaintballWorld.Infrastructure.Models.EventId;


namespace PaintballWorld.API.Areas.Event.Controllers
{
    [Route("api/[Area]/[controller]")]
    [Area("Event")]
    [ApiController]
    public class PublicEventController(ApplicationDbContext context, IAuthTokenService authTokenService) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Pobierz publiczne eventy
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<IActionResult> GetPublicEvents()
        {
            // nazwa, data, godzina, ilośćzapisana, ilośćmaksymalna
            var result = _context.Events.Where(x => x.IsPublic && x.StartDate > DateTime.UtcNow).OrderBy(x => x.StartDate).Take(100)
                .Select(x=> new PublicEvent
                {
                    Name = x.Name,
                    EventId = x.Id,
                    Date = x.StartDate,
                    SignedPlayers = x.UsersToEvents.Count,
                    MaxPlayers = x.MaxPlayers
                }).ToList();

            return Ok(result);

        }

        /// <summary>
        /// Pobierz publiczne eventy po polach
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{fieldId:guid}")]
        public async Task<IActionResult> GetPublicEvents([FromRoute] Guid fieldId)
        {
            var obj = new FieldId(fieldId);
            // nazwa, data, godzina, ilośćzapisana, ilośćmaksymalna
            var result = _context.Events.Where(x => x.IsPublic && x.FieldId == obj && x.StartDate > DateTime.UtcNow).OrderBy(x => x.StartDate).Take(100)
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

        /// <summary>
        /// Zapisz się na event (rozgrywkę otwartą)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreatePublicEvent(SignUpEventModel model)
        {
            try
            {
                var userId = authTokenService.GetUserId(User.Claims);
                var ev = context.Events.Include(@event => @event.Field).ThenInclude(field => field.Sets)
                    .FirstOrDefault(x => x.Id == new EventId(model.EventId));

                if (ev is null)
                    throw new Exception("Event not found");

                if (ev.Field.Sets.All(x => x.Id != new SetId(model.SetId)))
                    throw new Exception("Set not found for this field");

                ev.UsersToEvents.Add(new UsersToEvent
                {
                    EventId = ev.Id,
                    SetId = new SetId(model.SetId),
                    UserId = userId,
                    JoinedOnUtc = DateTime.UtcNow
                });
                await context.SaveChangesAsync();

                return Ok("User signed up for event");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /*        /// <summary>
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
                }*/

        /// <summary>
        /// usuń event
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpDelete]
        public async Task<IActionResult> DeletePublicEvent([FromQuery] Guid eventId)
        {
            var userId = authTokenService.GetUserId(User.Claims);

            var ev  = _context.Events.FirstOrDefault(x => x.Id == new EventId(eventId) && (x.CreatedBy == userId || x.Field.Owner.UserId.ToString() == userId));

            if (ev is not null)
            {
                _context.Events.Remove(ev);
            }

            return BadRequest("You cannot delete this event or event does not exist");

        }

        /// <summary>
        /// Wypisz się z eventu
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> SingOutFromEvent([FromQuery] Guid eventId)
        {
            try
            {
                var userId = authTokenService.GetUserId(User.Claims);
                var ev = context.Events.Include(x => x.UsersToEvents).FirstOrDefault(x => x.Id == new EventId(eventId));
                if (ev is null)
                    throw new Exception("Event not found");

                var ute = ev.UsersToEvents.FirstOrDefault(x => x.UserId == userId);

                if (ute is null)
                    throw new Exception("User was not signed to this event");

                ev.UsersToEvents.Remove(ute);
                await context.SaveChangesAsync();

                return Ok("User signed up for event");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
