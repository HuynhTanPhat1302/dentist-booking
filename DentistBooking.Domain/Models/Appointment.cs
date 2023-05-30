using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class Appointment
    {
        public Appointment()
        {
            AppointmentDetails = new HashSet<AppointmentDetail>();
        }

        public int AppointmentId { get; set; }
        public int? PatientId { get; set; }
        public int? DentistId { get; set; }
        public int? StaffId { get; set; }
        public DateTime? Datetime { get; set; }
        public double? Duration { get; set; }
        public string? Status { get; set; }

        public virtual Dentist? Dentist { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual staff? Staff { get; set; }
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }
    }
}
