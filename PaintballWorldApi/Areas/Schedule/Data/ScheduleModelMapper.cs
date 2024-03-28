using MimeKit.Text;
using PaintballWorld.API.Areas.Schedule.Models;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Schedule.Data;

public static class ScheduleModelMapper
{
    public static ScheduleModel Map(this CreateSchedulesDto dto, FieldId fieldId)
    {
        var result = new ScheduleModel
        {
            FieldId = fieldId,
            Schedules = dto.ScheduleDates.Select(x => new ScheduleItem
            {
                DateTime = x.ScheduleDate,
                MaxPlayTime = x.MaxPlayTime,
                MaxPlayers = x.MaxPlayers,
                FieldScheduleId = null

            }).ToList()
        };
        
        return result;
    }
}