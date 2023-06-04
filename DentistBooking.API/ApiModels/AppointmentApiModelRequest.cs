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
        [MinLength(1)]
        public double? Duration { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z]{7}$")]
        public string? Status { get; set; }
    }
}
