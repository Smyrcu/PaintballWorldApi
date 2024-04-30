using PaintballWorld.Core.Models;

namespace PaintballWorld.Core.Interfaces
{
    public interface IReservationService
    {
        Task Create(EventModel model);
        IList<EventModel> GetFieldReservations(Guid? fieldId, string? userId);
        Task DeleteReservation(Guid eventId, string? userId);
        Task EditReservation(EventModel model, string userId);
    }
}
