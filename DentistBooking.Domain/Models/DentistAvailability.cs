using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class DentistAvailability
    {
        public int AvailabilityId { get; set; }
        public int? DentistId { get; set; }
        public string? DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public virtual Dentist? Dentist { get; set; }
    }
}
