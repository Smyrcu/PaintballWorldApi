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
            EventType = dto.EventType,
            SelectedDays = dto.SelectedDays,
            IsRecurrent = dto.IsRecurrent,
            FinalDate = dto.FinalDate,
            Name = dto.Name,
            Date = dto.Date,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Description = dto.Description,
            TimeValue = dto.TimeValue,
            IsMultiple = dto.IsMultiple,
            IsWeekly = dto.IsWeekly,
            IsAutomatic = dto.IsAutomatic,
        };
        
        return result;
    }
}