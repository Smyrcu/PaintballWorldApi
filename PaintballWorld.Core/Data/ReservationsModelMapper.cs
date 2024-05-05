using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Data
{
    public static class ReservationsModelMapper
    {
        public static IList<EventModel> Map(this IList<Event> model) 
            => model.Select(x => new EventModel
            {
                EventId = x.Id.Value,
                UserId = x.CreatedBy,
                ScheduleId = null,
                SetId = null,
                isPrivate = !x.IsPublic,
                Description = x.Description ?? "",
                UsersInEvent = x != null && x.UsersToEvents.Any() ? x.UsersToEvents.Select(x => x.UserId).ToList() : []
            }).ToList();

        public static I
    }
}
