using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        var field = _context.Fields.Include(field => field.Owner).FirstOrDefault(x => x.Id == model.FieldId);
        
        if (field == null)
            throw new Exception("Field not found");

        if (model.EventType is null)
            throw new Exception("Event type cannot be null");

        if (model.EventType.ToLower().Equals("open"))
        {
            switch (model)
            {
                case { IsMultiple: true, IsWeekly: true }:
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
                                throw new Exception("StartTime needs to be Set");
                            if (model.EndTime is null)
                                throw new Exception("EndTime needs to be Set if IsAutomatic is true");

                            DateTime startDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                                model.StartTime.Value.Hour, model.StartTime.Value.Minute, model.StartTime.Value.Second);
                            DateTime endDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                                model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second);

                            if (startDate > model.FinalDate)
                                break;

                            var ev = new Event
                            {
                                FieldId = field.Id,
                                Description = model.Description,
                                CreatedBy = field.Owner.UserId.ToString(),
                                Name = model.Name,
                                MaxPlayers = model.MaxPlayers ?? field.MaxPlayers,
                                IsPublic = true,
                                StartDate = startDate,
                                EndDate = endDate,
                                CreatedOnUtc = DateTime.UtcNow,
                                UsersToEvents = []
                            };
                            _context.Events.Add(ev);
                        }
                        today += TimeSpan.FromDays(7);

                    } while (model.IsWeekly && today < model.FinalDate);

                    break;
                }
                case { IsMultiple: true, IsWeekly: false }:
                {
                    var today = DateTime.Today;
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
                            throw new Exception("StartTime needs to be Set");
                        if (model.EndTime is null)
                            throw new Exception("EndTime needs to be Set if IsAutomatic is true");

                        DateTime startDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                            model.StartTime.Value.Hour, model.StartTime.Value.Minute, model.StartTime.Value.Second);
                        DateTime endDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                            model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second);

                        var ev = new Event
                        {
                            FieldId = field.Id,
                            Description = model.Description,
                            CreatedBy = field.Owner.UserId.ToString(),
                            Name = model.Name,
                            MaxPlayers = model.MaxPlayers ?? field.MaxPlayers,
                            IsPublic = true,
                            StartDate = startDate,
                            EndDate = endDate,
                            CreatedOnUtc = DateTime.UtcNow,
                            UsersToEvents = []
                        };
                        _context.Events.Add(ev);
                    }

                    break;
                }
                case { IsMultiple: false, IsWeekly: true }:
                {
                    var today = DateTime.Today;
                    do
                    {
                        var dayOfWeek = model.Date.Value.DayOfWeek;
                        var daysUntilNext = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
                        if (daysUntilNext == 0)
                        {
                            daysUntilNext = 7;
                        }

                        var nextDate = today.AddDays(daysUntilNext);



                        if (model.StartTime is null)
                            throw new Exception("StartTime needs to be Set");
                        if (model.EndTime is null)
                            throw new Exception("EndTime needs to be Set if IsAutomatic is true");

                        DateTime startDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                            model.StartTime.Value.Hour, model.StartTime.Value.Minute, model.StartTime.Value.Second);
                        DateTime endDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                            model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second);

                        if(startDate > model.FinalDate)
                            break;

                        var ev = new Event
                        {
                            FieldId = field.Id,
                            Description = model.Description,
                            CreatedBy = field.Owner.UserId.ToString(),
                            Name = model.Name,
                            MaxPlayers = model.MaxPlayers ?? field.MaxPlayers,
                            IsPublic = true,
                            StartDate = startDate,
                            EndDate = endDate,
                            CreatedOnUtc = DateTime.UtcNow,
                            UsersToEvents = []
                        };
                        _context.Events.Add(ev);

                        today += TimeSpan.FromDays(7);

                    } while (today < model.FinalDate);

                    break;
                }
                default:
                {
                    var ev = new Event
                    {
                        FieldId = field.Id,
                        Description = model.Description,
                        CreatedBy = field.Owner.UserId.ToString(),
                        Name = model.Name,
                        MaxPlayers = model.MaxPlayers ?? field.MaxPlayers,
                        IsPublic = true,
                        StartDate = new DateTime(DateOnly.FromDateTime(model.Date.Value), model.StartTime.Value),
                        EndDate = new DateTime(DateOnly.FromDateTime(model.Date.Value), model.EndTime.Value),
                        CreatedOnUtc = DateTime.UtcNow,
                        UsersToEvents = [],

                    };
                    _context.Events.Add(ev);
                        break;
                }
            }
            await _context.SaveChangesAsync();
            return;
        }

        switch (model)
        {
            case { IsMultiple: true, IsWeekly: true, IsAutomatic: false }:
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

                        if (DateOnly.FromDateTime(nextDate) > DateOnly.FromDateTime(model.FinalDate.Value))
                            break;

                        if (model.StartTime is null)
                            throw new Exception("StartTime needs to be Set");
                        if (model.EndTime is null)
                            throw new Exception("EndTime needs to be Set if IsAutomatic is true");

                        DateTime startDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                            model.StartTime.Value.Hour, model.StartTime.Value.Minute, model.StartTime.Value.Second);
                        DateTime endDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                            model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second);

                        if (model.IsAutomatic && startDate <= model.FinalDate)
                        {
                            
                            await CreateAutomaticSchedules(model.TimeValue, startDate, model.EndTime, model.FieldId,
                                model.MaxPlayers ?? field.MaxPlayers);
                        }
                        else
                        {
                            var fieldSchedule = new FieldSchedule
                            {
                                FieldId = field.Id,
                                Date = startDate,
                                MaxPlayers = model.MaxPlayers ?? field.MaxPlayers,
                                MaxPlaytime = model.EndTime.Value - model.StartTime.Value,
                            };
                            _context.FieldSchedules.Add(fieldSchedule);
                        }
                    }

                    today += TimeSpan.FromDays(7);

                } while (model.IsWeekly && today < model.FinalDate);

                break;
            }
            case {IsMultiple: false, IsWeekly: true, IsAutomatic: false}:
            {
                var today = DateTime.Today;
                do
                {
                    var dayOfWeek = model.Date.Value.DayOfWeek;
                    var daysUntilNext = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
                    if (daysUntilNext == 0)
                    {
                        daysUntilNext = 7;
                    }

                    var nextDate = today.AddDays(daysUntilNext);

                    if (DateOnly.FromDateTime(nextDate) > DateOnly.FromDateTime(model.FinalDate.Value))
                        break;

                    if (model.StartTime is null)
                        throw new Exception("StartTime needs to be Set");
                    if (model.EndTime is null)
                        throw new Exception("EndTime needs to be Set if IsAutomatic is true");

                    DateTime startDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                        model.StartTime.Value.Hour, model.StartTime.Value.Minute, model.StartTime.Value.Second);
                    DateTime endDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                        model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second);

                    if(startDate <= model.FinalDate)
                    {
                        var fieldSchedule = new FieldSchedule
                        {
                            FieldId = field.Id,
                            Date = startDate,
                            MaxPlayers = model.MaxPlayers ?? field.MaxPlayers,
                            MaxPlaytime = model.EndTime.Value - model.StartTime.Value,
                        };
                        _context.FieldSchedules.Add(fieldSchedule);
                    }

                    today += TimeSpan.FromDays(7);

                } while (today < model.FinalDate);

                break;
            }
            case { IsMultiple: true, IsWeekly: false, IsAutomatic: false}:
            {
                var today = DateTime.Today;
                foreach (var selectedDay in model.SelectedDays)
                {
                    var dayOfWeek = Enum.Parse<DayOfWeek>(selectedDay);
                    var daysUntilNext = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
                    if (daysUntilNext == 0)
                    {
                        daysUntilNext = 7;
                    }

                    var nextDate = today.AddDays(daysUntilNext);

                    if (DateOnly.FromDateTime(nextDate) > DateOnly.FromDateTime(model.FinalDate.Value))
                        break;

                    if (model.StartTime is null)
                        throw new Exception("StartTime needs to be Set");
                    if (model.EndTime is null)
                        throw new Exception("EndTime needs to be Set if IsAutomatic is true");

                    DateTime startDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                        model.StartTime.Value.Hour, model.StartTime.Value.Minute, model.StartTime.Value.Second);
                    DateTime endDate = new(nextDate.Year, nextDate.Month, nextDate.Day,
                        model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second);

                    if (model.IsAutomatic && startDate <= model.FinalDate)
                    {
                        await CreateAutomaticSchedules(model.TimeValue, startDate, model.EndTime, model.FieldId,
                            model.MaxPlayers ?? field.MaxPlayers);
                    }
                    else
                    {
                        var fieldSchedule = new FieldSchedule
                        {
                            FieldId = field.Id,
                            Date = startDate,
                            MaxPlayers = model.MaxPlayers ?? field.MaxPlayers,
                            MaxPlaytime = model.EndTime.Value - model.StartTime.Value,
                        };
                        _context.FieldSchedules.Add(fieldSchedule);
                    }
                }

                break;
            }
            default:
            {
                if (model.IsAutomatic)
                {
                    switch (model)
                    {
                        case { IsWeekly: false, IsMultiple: false }:
                            await CreateAutomaticSchedules(model.TimeValue, model.Date, model.EndTime, model.FieldId, model.MaxPlayers ?? field.MaxPlayers);
                            break;
                        case { IsWeekly: true, StartTime: not null, IsMultiple: false}:
                        {
                            do
                            {
                                await CreateAutomaticSchedules(model.TimeValue, model.Date, model.EndTime, model.FieldId,
                                    model.MaxPlayers ?? model.MaxPlayers ?? field.MaxPlayers);
                                model.Date.Value.AddDays(7);
                            } while (model.Date < model.FinalDate);

                            break;
                        }
                        case { IsMultiple: true, IsWeekly: false }:
                        {
                            var today = DateTime.Today;
                            foreach (var selectedDay in model.SelectedDays)
                            {
                                var dayOfWeek = Enum.Parse<DayOfWeek>(selectedDay);
                                var daysUntilNext = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
                                if (daysUntilNext == 0)
                                {
                                    daysUntilNext = 7;
                                }

                                var nextDate = today.AddDays(daysUntilNext);

                                await CreateAutomaticSchedules(model.TimeValue, nextDate + model.StartTime.Value.ToTimeSpan(), model.EndTime, model.FieldId,
                                    model.MaxPlayers ?? field.MaxPlayers);

                            }

                            break;
                        }
                        case { IsMultiple: true, IsWeekly: true }:
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

                                    if(nextDate > model.FinalDate)
                                        break;

                                    await CreateAutomaticSchedules(model.TimeValue, nextDate + model.StartTime.Value.ToTimeSpan(), model.EndTime, model.FieldId,
                                        model.MaxPlayers ?? field.MaxPlayers);
                                    
                                }
                                today += TimeSpan.FromDays(7);
                            } while (today < model.FinalDate);

                            break;
                        }
                    }
                }
                else
                {
                    var time = model.EndTime.Value - model.StartTime.Value;
                    var fieldSchedule = new FieldSchedule
                    {
                        FieldId = field.Id,
                        Date = new DateTime(DateOnly.FromDateTime(model.Date.Value), model.StartTime.Value),
                        MaxPlayers = model.MaxPlayers ?? field.MaxPlayers,
                        MaxPlaytime = time,
                    };
                    _context.FieldSchedules.Add(fieldSchedule);
                }

                break;
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task CreateAutomaticSchedules(int? TimeValue, DateTime? StartTime, TimeOnly? EndTime, FieldId FieldId, int maxPlayers)
    {

        if (TimeValue is null)
            throw new Exception("TimeValue needs to be Set if IsAutomatic is true");
        if (StartTime is null)
            throw new Exception("StartTime needs to be Set if IsAutomatic is true");
        if (EndTime is null)
            throw new Exception("EndTime needs to be Set if IsAutomatic is true");

        var startTime = StartTime.Value;

        while (TimeOnly.FromDateTime(startTime).Add(TimeSpan.FromHours((double)TimeValue)) <= EndTime.Value)
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
        var result = await _context.FieldSchedules.Where(x => x.FieldId == fieldIdObj && x.Date >= DateTime.Today)
            .OrderBy(x => x.Date).Take(100).ToListAsync();
        
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