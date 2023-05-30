using System;
using System.Collections.Generic;

namespace DentistBooking.Infrastructure
{
    public partial class Illness
    {
        public Illness()
        {
            MedicalRecords = new HashSet<MedicalRecord>();
        }

        public int IllnessId { get; set; }
        public string? IllnessName { get; set; }

        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
