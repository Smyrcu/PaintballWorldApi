using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.API.Areas.Owner.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;
using System.Linq;
using EventId = PaintballWorld.Infrastructure.Models.EventId;

namespace PaintballWorld.API.Areas.Owner.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Owner")]
    [ApiController]
    public class MyEventsController (ApplicationDbContext context, IAuthTokenService authTokenService) : ControllerBase
    {
        /// <summary>
        /// Lista wydarzeń ownera - nie tykać
        /// </summary>
        /// <returns></returns>
    /*    [HttpGet]
        public async Task<IActionResult> GetMyEvents()
        {
            var userId = authTokenService.GetUserId(User.Claims);


            return Ok();
        }*/


        /// <summary>
        /// Szczegóły wydarzenia dla ownera
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMyEventDetails([FromRoute]Guid eventId)
        {
            try
            {
                var userId = authTokenService.GetUserId(User.Claims);

                var ev = context.Events.Include(@event => @event.Field).ThenInclude(field => field.Owner)
                    .Include(@event => @event.UsersToEvents).Include(@event => @event.CreatedByUser).FirstOrDefault(x => x.Id == new EventId(eventId));

                if (ev is null)
                    throw new Exception("Event not found");

                if (ev.Field.Owner.UserId.ToString() != userId)
                    throw new Exception("User is not the owner of field");
                var model = new MyEventModel
                {
                    EventId = ev.Id.Value,
                    CreatedBy = ev.CreatedBy,
                    ContactEmail = ev.CreatedByUser?.Email,
                    isPublic = ev.IsPublic,
                };

                var createdBy = context.UserInfos.First(x => x.UserId == ev.CreatedBy);

                model.CreatedbyName = createdBy.FirstName + " " + createdBy.LastName;
                model.ParticipantsCount = ev.UsersToEvents.Count;

                if (ev.IsPublic)
                {
                    model.Participants = ev.UsersToEvents.Select(x => new Participant
                    {
                        SetId = x.SetId?.Value,
                        Name = context.UserInfos.Where(y => y.UserId == x.UserId).Select(z => z.FirstName + " " + z.LastName).First()
                    }).ToList();

                    model.EstimatedPrice = 0;

                    foreach (var participant in model.Participants)
                    {
                        if(participant.SetId is null)
                            continue;

                        var set = context.Sets.First(x => x.Id == new SetId(participant.SetId.Value));


                        model.EstimatedPrice += set.Price ?? 0;
                    }
                }
                else if (!ev.IsPublic) // prywartne
                {
                    model.Participants = null;

                    var setId = ev.UsersToEvents.Select(x => x.SetId).First();

                    if (setId is null)
                        throw new Exception("Set not found");

                    var set = context.Sets.First(x => x.Id == setId);

                    var price = model.ParticipantsCount * set.Price;

                    if (price is null)
                        throw new Exception("Price is null");

                    model.EstimatedPrice = price.Value;
                }
                return Ok(model);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

    }
}
