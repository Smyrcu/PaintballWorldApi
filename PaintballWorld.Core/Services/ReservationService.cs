using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Services
{
    public class ReservationService(ApplicationDbContext context) : IReservationService
    {

        public async Task Create(EventModel model)
        {
            var fieldSchedule = context.FieldSchedules.Include(fieldSchedule => fieldSchedule.Field)
                .ThenInclude(field => field.FieldType).FirstOrDefault(x => x.Id == model.ScheduleId);

            if (fieldSchedule is null)
                throw new Exception("Field Schedule not found");

            if (!context.Sets.Any(x => x.Id == model.SetId && x.FieldId == fieldSchedule.FieldId))
                throw new Exception("Set with this SetId not found for selected Field");


            var ev = new Event
            {
                FieldTypeId = fieldSchedule.Field.FieldTypeId,
                FieldType = fieldSchedule.Field.FieldType,
                Description = model.Description,
                CreatedBy = model.UserId,
                IsPublic = !model.isPrivate,
                Date = DateOnly.FromDateTime(fieldSchedule.Date),
                Time = TimeOnly.FromDateTime(fieldSchedule.Date),
                CreatedOnUtc = DateTime.UtcNow,
                UsersToEvents = new List<UsersToEvent>
                {
                    new()
                    {
                        SetId = model.SetId,
                        UserId = model.UserId,
                        JoinedOnUtc = DateTime.UtcNow
                    }
                }
            };

            context.Events.Add(ev);
            await context.SaveChangesAsync();
        }
    }
}
