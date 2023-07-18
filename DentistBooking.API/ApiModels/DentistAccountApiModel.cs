using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public class DentistAccountApiModel
    {
        [Required]
        [UniqueDentistEmail]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string? DentistName { get; set; }

        [RegularExpression("^(?:\\+?84|0)(?:1\\d{9}|3\\d{8}|5\\d{8}|7\\d{8}|8\\d{8}|9\\d{8})$")]
        public string? PhoneNumber { get; set; }
    }
}
