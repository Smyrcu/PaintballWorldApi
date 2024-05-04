using Org.BouncyCastle.Crypto.Agreement.JPake;

namespace PaintballWorld.API.Areas.Owner.Models
{
    public class MyEventModel
    {
        public Guid EventId { get; set; }
        public string CreatedBy { get; set; }
        public string? ContactEmail { get; set; }
        public string CreatedbyName { get; set; }
        public bool isPublic { get; set; }
        public decimal EstimatedPrice { get; set; }
        public int ParticipantsCount { get; set; }
        public List<Participant>? Participants { get; set; } = [];

    }

    public class Participant
    {
        public Guid? SetId { get; set; }
        public string Name { get; set; }
    }
}
