using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Data;

public static class ScheduleModelMapper
{
    public static ScheduleModel Map(this IEnumerable<FieldSchedule> fs)
    {
        var result = new ScheduleModel
        {
            FieldId = fs.First().FieldId,
            Schedules = fs.Select(x => new ScheduleItem
            {
                DateTime = x.Date,
                FieldScheduleId = x.Id
            }).ToList()
        };
        return result;
    }
    
}