using EventId = PaintballWorld.Infrastructure.Models.EventId;

namespace PaintballWorld.API.Areas.Event.Models
{
    public class PublicEvent
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int SignedPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public EventId EventId { get; set; }
    }
}
