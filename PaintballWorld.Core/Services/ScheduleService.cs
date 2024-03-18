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

    public async Task AddSchedules(FieldId fieldId, ScheduleModel dto)
    {
        List<FieldSchedule> schedulesList = [];
        
        dto.Schedules.ForEach(x =>
        {
            schedulesList.Add(new FieldSchedule
            {
                FieldId = fieldId,
                Date = x.DateTime,
                IsRecurrent = false,
                LastUpdatedUtc = default,
                CreatedUtc = default
            });
        });
        
        await _context.FieldSchedules.AddRangeAsync(schedulesList);
        await _context.SaveChangesAsync();
    }

    public async Task<ScheduleModel> GetSchedulesByField(FieldId fieldIdObj)
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