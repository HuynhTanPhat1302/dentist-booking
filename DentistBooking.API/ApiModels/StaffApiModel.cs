using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class StaffApiModel
        {
            [Required]
            public string? StaffName { get; set; }
            [Required]
            public string? PhoneNumber { get; set; }
            [Required]
            public string? Email { get; set; }
        }
    }

}
