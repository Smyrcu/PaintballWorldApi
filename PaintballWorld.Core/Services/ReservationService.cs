using Microsoft.EntityFrameworkCore;
using PaintballWorld.Core.Data;
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

        public IEnumerable<EventModel> GetFieldReservations(Guid? fieldId, string? userId)
        {
            if(fieldId is null && userId is null)
                throw new Exception("No data provided");

            IQueryable<Event> result;

            if (fieldId is not null && userId is null)
            {
                result = context.Events.Where(x => x.FieldId == new FieldId(fieldId.Value));
                return result.AsEnumerable().Map();

            }

            if (userId is not null && fieldId is null)
            {
                result = context.Events.Where(x => x.UsersToEvents.Any(x => x.UserId == userId));
                return result.AsEnumerable().Map();
            }

            result = context.Events.Where(x =>
                x.FieldId == new FieldId(fieldId.Value) && x.UsersToEvents.Any(x => x.UserId == userId));
            return result.AsEnumerable().Map();
        }

        public async Task DeleteReservation(Guid eventId, string? userId)
        {
            var ev = context.Events.Include(@event => @event.Field).ThenInclude(field => field.Owner)
                .FirstOrDefault(x => x.Id == new EventId(eventId));

            if (ev != null)
            {
                if (ev.Field.Owner.UserId.ToString() == userId)
                {
                    context.Events.Remove(ev);
                    await context.SaveChangesAsync();
                }
                else if (ev.CreatedBy == userId)
                {
                    context.Events.Remove(ev);
                    await context.SaveChangesAsync();
                }

                throw new Exception("User is not owner of field or event - cannot delete");
            }

        }

        public async Task EditReservation(EventModel model, string userId)
        {
            var ev = context.Events.Include(@event => @event.Field).ThenInclude(field => field.Owner).FirstOrDefault(x => x.Id == model.EventId);
            if (ev != null)
            {
                if (ev.Field.Owner.UserId.ToString() == userId)
                {
                    ev.IsPublic = !model.isPrivate;
                    ev.Description = model.Description;
                    await context.SaveChangesAsync();
                }
                else if (ev.CreatedBy == userId)
                {
                    ev.IsPublic = !model.isPrivate;
                    ev.Description = model.Description;
                    await context.SaveChangesAsync();
                }

                throw new Exception("User is not owner of field or event - cannot delete");
            }

        }
    }
}
