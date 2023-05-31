using Microsoft.Build.Framework;

namespace DentistBooking.API.ApiModels
{
    public class PatientApiModelRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PatientName { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string? PatientCode { get; set; }
        [Required]
        public string? Address { get; set; }
    }
}
