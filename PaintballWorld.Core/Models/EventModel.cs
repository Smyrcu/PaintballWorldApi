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
        public string UserId { get; set; }
        public FieldScheduleId ScheduleId { get; set; }
        public SetId SetId { get; set; }
        public bool isPrivate { get; set; } = false;
        public string? Description { get; set; }
    }
}
