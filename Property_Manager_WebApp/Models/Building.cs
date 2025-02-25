using System;
using System.Collections.Generic;

namespace Property_Manager_WebApp.Models;

public partial class Building
{
    public int BuildingId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Description { get; set; }

    public int ManagerId { get; set; }

    public int OwnerId { get; set; } // New property

    public virtual ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();

    public virtual User Manager { get; set; } = null!;

    public virtual User Owner { get; set; } = null!; // Navigation property for Owner
}
