using System;

namespace Property_Manager_WebApp.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int ManagerId { get; set; }

    public string DayOfWeek { get; set; } = null!; // e.g., "Monday", "Tuesday"

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual User Manager { get; set; } = null!;
}
