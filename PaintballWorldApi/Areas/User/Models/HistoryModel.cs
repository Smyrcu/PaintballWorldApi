namespace PaintballWorld.API.Areas.User.Models
{
    public class HistoryModel
    {
        public int GamesPlayed { get; set; }
        public List<Game> games { get; set; } = [];

    }

    public class Game
    {
        public Field Field { get; set; }
        public Event Event { get; set; }
        public bool IsPublic { get; set; }
    }

    public class Field
    {
        public Guid FieldId { get; set; }
        public string? FieldName { get; set; }
        public string? City { get; set; }
    }

    public class Event
    {
        public Guid EventId { get; set; }
        public int? Ammo { get; set; }
        public decimal? Price { get; set; }
        public DateTime Date { get; set; }
    }
}
