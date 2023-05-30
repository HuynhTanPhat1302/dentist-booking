using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class ProposeAppointment
    {
        public int ProposeAppointmentId { get; set; }
        public DateTime? Datetime { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
    }
}
