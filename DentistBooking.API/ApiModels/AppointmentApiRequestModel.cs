using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public class AppointmentApiRequestModel
    {
        [Required]
        public string? PatientName { get; set; }
        [Required]
        public string? DentistName { get; set; }
        [Required]
        public string? StaffName { get; set; }
        [Required]
        public DateTime? Datetime { get; set; }
        
        [Required]
        public double? Duration { get; set; }

        [Required]
        public string? Status { get; set; }
    }
}
