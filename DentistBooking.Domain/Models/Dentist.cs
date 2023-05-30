using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class Dentist
    {
        public Dentist()
        {
            Appointments = new HashSet<Appointment>();
            DentistAvailabilities = new HashSet<DentistAvailability>();
            MedicalRecords = new HashSet<MedicalRecord>();
        }

        public int DentistId { get; set; }
        public string? Email { get; set; }
        public string? DentistName { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<DentistAvailability> DentistAvailabilities { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
