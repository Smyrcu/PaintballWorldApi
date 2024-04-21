using Microsoft.Build.Framework;
using PaintballWorld.API.Areas.Schedule.Models;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Schedule.Data
{
    public static class ReservationModelMapper
    {
        public static EventModel Map(this CreateReservationDto dto)
        {
            return new EventModel
            {
                UserId = dto.UserId,
                ScheduleId = new FieldScheduleId(dto.ScheduleId),
                SetId = new SetId(dto.SetId),
                isPrivate = dto.isPrivate,
                Description = dto.Description,

            };
        }
    }
}
