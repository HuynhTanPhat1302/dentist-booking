using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class Treatment
    {
        public Treatment()
        {
            MedicalRecords = new HashSet<MedicalRecord>();
        }

        public int TreatmentId { get; set; }
        public string? TreatmentName { get; set; }
        public decimal? Price { get; set; }
        public double? EstimatedTime { get; set; } //hours
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
