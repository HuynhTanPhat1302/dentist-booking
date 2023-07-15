
using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public class RegisterRequestModel
    {
        public RegisterRequestModel()
        {
            CreatedAt = DateTime.Now;
        }

        [Required]
        [EmailAddress]
        [UniquePatientEmail]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string? PatientName { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$")]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [MinLength(6)]
        [UniquePatientCode]
        [RegularExpression("^P\\d{6}$")]
        public string? PatientCode { get; set; }
        
        [Required]
        public string? Address { get; set; }
    }
}
