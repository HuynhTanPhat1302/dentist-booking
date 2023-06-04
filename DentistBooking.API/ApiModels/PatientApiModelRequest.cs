
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public class PatientApiModelRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z]{7}$")]
        public string? PatientName { get; set; }
        [Required]
        [MinLength(10)]
        [RegularExpression("^(?:\\+?84|0)(?:1\\d{9}|3\\d{8}|5\\d{8}|7\\d{8}|8\\d{8}|9\\d{8})$")]
        public string? PhoneNumber { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [MinLength(6)]
        [RegularExpression("^P\\d{6}$")]
        public string? PatientCode { get; set; }
        [Required]
        public string? Address { get; set; }
    }
}
