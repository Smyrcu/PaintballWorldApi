using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Schedule.Models
{
    public class PrivateEvent
    {
        public DateTime Date { get; set; }
        public int MaxPlayers { get; set; }
        public Guid FieldScheduleId { get; set; }
        public TimeSpan MaxPlaytime { get; set; }
    }
}
