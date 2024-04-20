using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Models;

public class ScheduleModel
{
    public FieldId FieldId { get; set; }
    public string EventType { get; set; }
    public List<string> SelectedDays { get; set; } = [];
    public bool IsRecurrent { get; set; }
    public string FinalDate { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Description { get; set; }
    public string TimeValue { get; set; }
    public bool IsMultiple { get; set; }
    public bool IsWeekly { get; set; }
    public bool IsAutomatic { get; set; }

}
