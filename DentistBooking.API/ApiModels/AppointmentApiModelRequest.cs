using DentistBooking.API.Validation;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public class AppointmentApiModelRequest
    {
        [Required]
        public int? PatientId { get; set; }
        [Required]
        public int? DentistId { get; set; }
        [Required]
        public int? StaffId { get; set; }
        [Required]
        public DateTime? Datetime { get; set; }
        [Required]
        [TreatmentDurationIsExistedOrNot]
        public double? Duration { get; set; }
        [Required]
        public string? Status { get; set; }
    }
}
