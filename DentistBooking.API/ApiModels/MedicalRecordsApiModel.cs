
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public class MedicalRecordsApiModel
    {

        [Required]
        public int MedicalRecordId { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int DentistId { get; set; }
        [Required]
        public int? TeethNumber { get; set; }
        [Required]
        public int IllnessID { get; set; }
        [Required]
        public int TreatmentID { get; set; }
        [Required]
        public string? Status { get; set; }
    }
}
