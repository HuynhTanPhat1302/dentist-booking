using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DentistBooking.Infrastructure;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class NestedMedicalRecordRespondModel {
            public int MedicalRecordId { get; set; }
            public int? PatientId { get; set; }
            public int? DentistId { get; set; }
            public int? TeethNumber { get; set; }
            public int? IllnessId { get; set; }
            public int? TreatmentId { get; set; }
            public string? Status { get; set; }
             public virtual DentistRepondModel? Dentist { get; set; }

            public virtual IllnessRespondModel? Illness { get; set; }
            public virtual PatientRespondModel? Patient { get; set; }

            public virtual TreatmentRespondModel? Treatment { get; set; }

            public virtual ICollection<AppointmentDetailRespondModel>? AppointmentDetails { get; set; }
        }
    }
}
