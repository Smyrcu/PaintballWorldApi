using PaintballWorld.API.Areas.Schedule.Models;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Event.Models
{
    public static class ReservationModelMapper
    {
        public static EventModel Map(this CreateReservationDto dto, string userId)
        {
            return new EventModel
            {
                ScheduleId = dto.ScheduleId,
                SetId = dto.SetId,
                isPrivate = dto.isPrivate,
                Description = dto.Description,
                PlayersCount = dto.PlayersCount,
                UserId = userId

            };
        }
    }
}
