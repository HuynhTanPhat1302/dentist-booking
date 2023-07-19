using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;


namespace DentistBooking.API.ApiModels
{
    public partial class MedicalRecordCreatedModel
    {
        
        [Required]
        [ValidId]
        [ExistedPatientId]
        public int? PatientId { get; set; }

        [Required]
        [ValidId]
        public int? DentistId { get; set; }

        [Required]
        [ValidTeethNumber]
        public int? TeethNumber { get; set; }

        [Required]
        [ValidId]
        [ExistedIllnessId]
        public int? IllnessId { get; set; }

        [Required]
        [ValidId]
        [ExistedTreatmentId]
        public int? TreatmentId { get; set; }

        
    }
}
