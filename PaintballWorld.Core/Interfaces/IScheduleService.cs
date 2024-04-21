using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IScheduleService
{
    Task AddSchedules(ScheduleModel model);
    Task<IEnumerable<ScheduleModel>> GetSchedulesByField(FieldId fieldIdObj);
    Task DeleteSchedule(FieldId fieldId, FieldScheduleId fieldScheduleId);
}