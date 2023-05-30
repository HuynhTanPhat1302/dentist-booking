using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class MedicalRecord
    {
        public MedicalRecord()
        {
            AppointmentDetails = new HashSet<AppointmentDetail>();
        }

        public int MedicalRecordId { get; set; }
        public int? PatientId { get; set; }
        public int? DentistId { get; set; }
        public int? TeethNumber { get; set; }
        public int? IllnessId { get; set; }
        public int? TreatmentId { get; set; }
        public string? Status { get; set; }

        public virtual Dentist? Dentist { get; set; }
        public virtual Illness? Illness { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual Treatment? Treatment { get; set; }
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }
    }
}
