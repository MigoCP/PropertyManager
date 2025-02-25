using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Property_Manager_WebApp.Models
{
    public partial class Apartment
    {
        public int ApartmentId { get; set; }

        [Required]
        public int BuildingId { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } = null!;

        [Range(1, 20)]
        public int NumberOfRooms { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public virtual Building? Building { get; set; }
    }
}
