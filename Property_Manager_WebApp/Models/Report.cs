using System;

namespace Property_Manager_WebApp.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int ManagerId { get; set; }

    public int OwnerId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime ReportDate { get; set; } = DateTime.Now;

    public string Status { get; set; } = "Pending";

    public virtual User? Manager { get; set; } = null!;

    public virtual User? Owner { get; set; } = null!;
}
