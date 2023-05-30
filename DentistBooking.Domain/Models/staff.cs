using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class staff
    {
        public staff()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int StaffId { get; set; }
        public string? StaffName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
