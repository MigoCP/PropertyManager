using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Property_Manager_WebApp.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    [Required]
    public int TenantId { get; set; }

    [Required]
    public int ManagerId { get; set; }

    [Required]
    public int ApartmentId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime ScheduledDate { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending";

    public virtual Apartment? Apartment { get; set; }
    public virtual User? Manager { get; set; }
    public virtual User? Tenant { get; set; }
}
