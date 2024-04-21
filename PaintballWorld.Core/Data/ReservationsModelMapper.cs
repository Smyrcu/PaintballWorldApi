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
        public static IEnumerable<EventModel> Map(this IEnumerable<Event> model)
        {
            return model.Select(x => new EventModel
            {
                UserId = x.CreatedBy,
                ScheduleId = null,
                SetId = default,
                isPrivate = !x.IsPublic,
                Description = x.Description,
                UsersInEvent = x.UsersToEvents.Select(x => x.UserId).ToList()
            });
        }
    }
}
