using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public class DentistAccountApiModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string? DentistName { get; set; }

        [RegularExpression("^(?:\\+?84|0)(?:1\\d{9}|3\\d{8}|5\\d{8}|7\\d{8}|8\\d{8}|9\\d{8})$")]
        public string? PhoneNumber { get; set; }
    }
}
