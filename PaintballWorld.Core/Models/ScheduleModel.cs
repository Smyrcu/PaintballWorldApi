using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Models;

public class ScheduleModel
{
    public FieldId FieldId { get; set; }
    public FieldScheduleId? ScheduleId { get; set; }
    public string? EventType { get; set; } // open/private
    public List<string> SelectedDays { get; set; } = [];//mondej tjuzdej itp
    public DateTime? FinalDate { get; set; } // jak się powtarza to do kiedy
    public string? Name { get; set; } // 
    public DateTime? Date { get; set; } // data skedżulera
    public DateTime? StartTime { get; set; } // 
    public DateTime? EndTime { get; set; }//
    public string? Description { get; set; } //opis
    public int? TimeValue { get; set; }
    public bool IsMultiple { get; set; } // czy to jest kilka dni w tygodniu
    public bool IsWeekly { get; set; } // czy się powtarza co tydz
    public bool IsAutomatic { get; set; } // czy ma tworzyć na podstawie godzin otwarcia / timevalue
    public int? MaxPlayers { get; set; }

}
