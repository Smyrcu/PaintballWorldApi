using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Data;

public static class ScheduleModelMapper
{
    public static IEnumerable<ScheduleModel> Map(this IEnumerable<FieldSchedule> fs)
    {
        var result = fs.Select(x => new ScheduleModel
        {
            FieldId = x.FieldId,
            ScheduleId = x.Id,
            Date = x.Date,
            StartTime = x.Date,
            EndTime = x.Date.Add(x.MaxPlaytime),
            MaxPlayers = x.MaxPlayers
        });
        return result;
    }
    
}