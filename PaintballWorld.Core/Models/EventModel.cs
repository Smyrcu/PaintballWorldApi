using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Models
{
    public class EventModel
    {
        public Guid EventId { get; set; }
        public string UserId { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? SetId { get; set; }
        public bool isPrivate { get; set; } = false;
        public string? Description { get; set; }
        public List<string> UsersInEvent { get; set; } = [];
    }
}
