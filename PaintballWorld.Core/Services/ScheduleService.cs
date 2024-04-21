using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaintballWorld.Core.Data;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Services;

public class ScheduleService : IScheduleService
{
    private readonly ILogger<ScheduleService> _logger;
    private readonly ApplicationDbContext _context;

    public ScheduleService(ILogger<ScheduleService> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task AddSchedules(ScheduleModel model)
    {
        var field = _context.Fields.FirstOrDefault(x => x.Id == model.FieldId);

        if(field == null)
            throw new Exception("Field not found");

        if (model.IsMultiple || model.IsWeekly)
        {
            var today = DateTime.Today;
            do
            {
                foreach (var selectedDay in model.SelectedDays)
                {
                    var dayOfWeek = Enum.Parse<DayOfWeek>(selectedDay);
                    var daysUntilNext = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
                    if (daysUntilNext == 0)
                    {
                        daysUntilNext = 7;
                    }

                    var nextDate = today.AddDays(daysUntilNext);

                    if (model.StartTime is null)
                        throw new Exception("StartTime needs to be set");
                    if (model.EndTime is null)
                        throw new Exception("EndTime needs to be set if IsAutomatic is true");

                    DateTime startDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                        model.StartTime.Value.Hour, model.StartTime.Value.Minute, model.StartTime.Value.Second);
                    DateTime endDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                        model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second);

                    if (model.IsAutomatic && startDate < model.FinalDate)
                    {
                        await CreateAutomaticSchedules(model.TimeValue, startDate, endDate, model.FieldId,
                            field.MaxPlayers);
                    }
                    else
                    {
                        var fieldSchedule = new FieldSchedule
                        {
                            FieldId = field.Id,
                            Date = startDate,
                            MaxPlayers = field.MaxPlayers,
                            MaxPlaytime = TimeSpan.FromHours((double)model.TimeValue),
                        };
                        _context.FieldSchedules.Add(fieldSchedule);
                    }
                }

                today += TimeSpan.FromDays(7);

            } while (model.IsWeekly && today < model.FinalDate);
        }
        else if (model.IsAutomatic)
        {
            await CreateAutomaticSchedules(model.TimeValue, model.StartTime, model.EndTime, model.FieldId, field.MaxPlayers);
        }
        else
        {
            var time = model.TimeValue ?? 10;
            var fieldSchedule = new FieldSchedule
            {
                FieldId = field.Id,
                Date = model.Date.Value,
                MaxPlayers = field.MaxPlayers,
                MaxPlaytime = TimeSpan.FromHours((double)time),
            };
            _context.FieldSchedules.Add(fieldSchedule);
        }

        await _context.SaveChangesAsync();
    }

    private async Task CreateAutomaticSchedules(int? TimeValue, DateTime? StartTime, DateTime? EndTime, FieldId FieldId, int maxPlayers)
    {

        if (TimeValue is null)
            throw new Exception("TimeValue needs to be set if IsAutomatic is true");
        if (StartTime is null)
            throw new Exception("StartTime needs to be set if IsAutomatic is true");
        if (EndTime is null)
            throw new Exception("EndTime needs to be set if IsAutomatic is true");

        var startTime = StartTime.Value;

        while (startTime < EndTime)
        {
            var fieldSchedule = new FieldSchedule
            {
                FieldId = FieldId,
                Date = startTime,
                MaxPlayers = maxPlayers,
                MaxPlaytime = TimeSpan.FromHours((double)TimeValue),
            };
            _context.FieldSchedules.Add(fieldSchedule);
            startTime += TimeSpan.FromHours((double)TimeValue);
        }
    } 

    public async Task<IEnumerable<ScheduleModel>> GetSchedulesByField(FieldId fieldIdObj)
    {
        var result = await _context.FieldSchedules.Where(x => x.FieldId == fieldIdObj && x.Date >= DateTime.Today).ToListAsync();
        
        var model = result.Map();

        return model;
    }

    public async Task DeleteSchedule(FieldId fieldId, FieldScheduleId fieldScheduleId)
    {
        var schedule = _context.FieldSchedules.Where(x => x.Id == fieldScheduleId);

        if (schedule.Any(x => x.FieldId != fieldId))
        {
            throw new Exception("Niepoprawne połączenie Field oraz Schedule.");
        }
        
        _context.FieldSchedules.RemoveRange(schedule);
        await _context.SaveChangesAsync();
    }
}