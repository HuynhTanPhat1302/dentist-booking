
using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public class PatientApiRequestModel
    {
        [Required]
        [EmailAddress]
        [UniquePatientEmail]
        public string? Email { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string? PatientName { get; set; }
        [Required]
       
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
