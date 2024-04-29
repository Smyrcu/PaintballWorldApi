﻿namespace PaintballWorld.API.Areas.Event.Models
{
    public class CreateReservationDto
    {
        public string UserId { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid SetId { get; set; }
        public bool isPrivate { get; set; } = false;
        public string? Description { get; set; }
    }
}
