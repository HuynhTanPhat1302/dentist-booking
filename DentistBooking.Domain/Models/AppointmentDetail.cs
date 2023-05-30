using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class AppointmentDetail
    {
        public int AppointmentDetailId { get; set; }
        public int? AppointmentId { get; set; }
        public int? MedicalRecordId { get; set; }

        public virtual Appointment? Appointment { get; set; }
        public virtual MedicalRecord? MedicalRecord { get; set; }
    }
}
