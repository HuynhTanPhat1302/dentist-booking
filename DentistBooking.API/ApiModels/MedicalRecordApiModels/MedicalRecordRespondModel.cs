using System.ComponentModel.DataAnnotations;
using DentistBooking.Infrastructure;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class MedicalRecordRespondModel {
            public int MedicalRecordId { get; set; }
            public int? PatientId { get; set; }
            public int? DentistId { get; set; }
            public int? TeethNumber { get; set; }
            public int? IllnessId { get; set; }
            public int? TreatmentId { get; set; }
            public string? Status { get; set; }

            // public virtual Dentist? Dentist { get; set; }
            // public virtual Illness? Illness { get; set; }
            // public virtual Patient? Patient { get; set; }
            // public virtual Treatment? Treatment { get; set; }
            // public virtual ICollection<AppointmentDetail>? AppointmentDetails { get; set; }
        }
    }
}
