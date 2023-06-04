using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public class MedicalRecordApiRequestModel
    {
        [Required]
        public string? PatientName { get; set; }
        [Required]
        public string? DentistName { get; set; }
        [Required]
        public int? TeethNumber { get; set; }
        [Required]
        public string? IllnessName { get; set; }
        [Required]
        public string? TreatmentName { get; set; }
        [Required]
        public string? Status { get; set; }
    }
}